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

namespace FLOG_BE.Features.Sales.Quotation.DeleteQuotation
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
                SalesQuotationId = request.Body.SalesQuotationId
            };
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var record = _context.SalesQuotationHeaders.Where(x => x.SalesQuotationId == request.Body.SalesQuotationId).FirstOrDefault();

                    if(record != null)
                    {
                        _context.QuotationDetails.Where(x => x.SalesQuotationId == request.Body.SalesQuotationId).ToList().ForEach(p => _context.Remove(p));
                        _context.SalesQuotationHeaders.Where(x => x.SalesQuotationId == request.Body.SalesQuotationId).ToList().ForEach(p => _context.Remove(p));

                        //UPDATE LAST NO
                        _docGenerator.DocNoDelete(record.DocumentNo, transaction.GetDbTransaction());

                        await _context.SaveChangesAsync();

                        transaction.Commit();

                        return ApiResult<Response>.Ok(response);
                    }
                    else
                    {
                        transaction.Rollback();

                        return ApiResult<Response>.ValidationError("Quotation not exist for deletion !");
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
