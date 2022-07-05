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

namespace FLOG_BE.Features.Sales.NegotiationSheet.DeleteNegotiationSheet
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
                NegotiationSheetId = request.Body.NegotiationSheetId
            };
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var record = _context.NegotiationSheetHeaders.Where(x => x.NegotiationSheetId == request.Body.NegotiationSheetId).FirstOrDefault();

                    if (record != null)
                    {
                        _context.NegotiationSheetBuyings.Where(x => x.NegotiationSheetId == request.Body.NegotiationSheetId).ToList().ForEach(p => _context.Remove(p));
                        _context.NegotiationSheetSellings.Where(x => x.NegotiationSheetId == request.Body.NegotiationSheetId).ToList().ForEach(p => _context.Remove(p));
                        _context.NegotiationSheetContainers.Where(x => x.NegotiationSheetId == request.Body.NegotiationSheetId).ToList().ForEach(p => _context.Remove(p));
                        _context.NegotiationSheetTruckings.Where(x => x.NegotiationSheetId == request.Body.NegotiationSheetId).ToList().ForEach(p => _context.Remove(p));
                        _context.NegotiationSheetHeaders.Where(x => x.NegotiationSheetId == request.Body.NegotiationSheetId).ToList().ForEach(p => _context.Remove(p));

                        //UPDATE LAST NO
                        _docGenerator.DocNoDelete(record.DocumentNo, transaction.GetDbTransaction());

                        //UPDATE SO HEADER
                        var so = _context.SalesOrderHeaders.Where(s => s.SalesOrderId == record.SalesOrderId).FirstOrDefault();
                        if(so != null)
                        {
                            so.Status = DOCSTATUS.NEW;
                            so.ModifiedBy = request.Initiator.UserId;
                            so.ModifiedDate = DateTime.Now;

                            await _context.SaveChangesAsync();
                            transaction.Commit();

                            return ApiResult<Response>.Ok(response);
                        }
                        else
                        {
                            transaction.Rollback();

                            return ApiResult<Response>.ValidationError("Sales Order not found !");
                        }
                    }
                    else
                    {
                        transaction.Rollback();

                        return ApiResult<Response>.ValidationError("Negotiation Sheet not exist for deletion !");
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
