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
using Entities = FLOG_BE.Model.Companies.Entities;
using FLOG.Core;
using FLOG.Core.DocumentNo;
using Infrastructure;

namespace FLOG_BE.Features.Finance.ArReceipt.DeleteCustomerReceipt
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
                ReceiptHeaderId = request.Body.ReceiptHeaderId
            };

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var Receipt = await _context.ArReceiptHeaders.FirstOrDefaultAsync(x => x.ReceiptHeaderId == request.Body.ReceiptHeaderId);

                    if (Receipt != null)
                    {
                        if (Receipt.Status == DOCSTATUS.NEW)
                        {
                            _context.ArReceiptDetails.Where(x => x.ReceiptHeaderId == request.Body.ReceiptHeaderId).ToList().ForEach(p => _context.Remove(p));
                            _context.ArReceiptHeaders.Where(x => x.ReceiptHeaderId == request.Body.ReceiptHeaderId).ToList().ForEach(p => _context.Remove(p));

                            _docGenerator.DocNoDelete(Receipt.DocumentNo, trans.GetDbTransaction());

                            await _context.SaveChangesAsync();

                            trans.Commit();
                        }
                        else
                        {
                            trans.Rollback();
                            return ApiResult<Response>.ValidationError("Customer Receipt can not be deleted.");

                        }
                    }
                    else
                    {
                        trans.Rollback();
                        return ApiResult<Response>.ValidationError("Customer Receipt not found!");

                    }

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return ApiResult<Response>.InternalServerError("Delete failed ! " + ex.Message);
                }
            }
        }
    }
}
