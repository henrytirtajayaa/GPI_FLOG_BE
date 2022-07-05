using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Companies;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using FLOG_BE.Model.Companies.Entities;
using FLOG.Core;
using FLOG.Core.Finance.Util;
using FLOG.Core.DocumentNo;
using Infrastructure;

namespace FLOG_BE.Features.Sales.SalesOrder.DeleteSalesOrder
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private IDocumentGenerator _docGenerator;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
            _docGenerator = new DocumentGenerator(_context);
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            var response = new Response()
            {
                SalesOrderId = request.Body.SalesOrderId
            };
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var record = _context.SalesOrderHeaders.Where(x => x.SalesOrderId == request.Body.SalesOrderId).FirstOrDefault();

                    if (record != null)
                    {

                        _context.SalesOrderBuyings.Where(x => x.SalesOrderId == request.Body.SalesOrderId).ToList().ForEach(p => _context.Remove(p));
                        _context.SalesOrderSellings.Where(x => x.SalesOrderId == request.Body.SalesOrderId).ToList().ForEach(p => _context.Remove(p));
                        _context.SalesOrderTruckings.Where(x => x.SalesOrderId == request.Body.SalesOrderId).ToList().ForEach(p => _context.Remove(p));
                        _context.SalesOrderContainers.Where(x => x.SalesOrderId == request.Body.SalesOrderId).ToList().ForEach(p => _context.Remove(p));
                        _context.SalesOrderHeaders.Where(x => x.SalesOrderId == request.Body.SalesOrderId).ToList().ForEach(p => _context.Remove(p));

                        //UPDATE LAST NO
                        _docGenerator.DocNoDelete(record.DocumentNo, transaction.GetDbTransaction());

                        await _context.SaveChangesAsync();
                        transaction.Commit();

                        return ApiResult<Response>.Ok(response);
                    }
                    else
                    {
                        transaction.Rollback();

                        return ApiResult<Response>.ValidationError("Sales Order not exist for deletion !");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ApiResult<Response>.ValidationError("Delete failed ! " + ex.Message);
                }
            }
        }
    }
}
