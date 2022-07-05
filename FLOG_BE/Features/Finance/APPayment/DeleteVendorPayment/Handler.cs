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

namespace FLOG_BE.Features.Finance.ApPayment.DeleteVendorPayment
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
                PaymentHeaderId = request.Body.PaymentHeaderId
            };
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var payment = await _context.ApPaymentHeaders.FirstOrDefaultAsync(x => x.PaymentHeaderId == request.Body.PaymentHeaderId);
                    if (payment != null)
                    {
                        if (payment.Status == DOCSTATUS.NEW)
                        {
                            _context.ApPaymentDetails.Where(x => x.PaymentHeaderId == request.Body.PaymentHeaderId).ToList().ForEach(p => _context.Remove(p));
                            _context.ApPaymentHeaders.Where(x => x.PaymentHeaderId == request.Body.PaymentHeaderId).ToList().ForEach(p => _context.Remove(p));

                            _docGenerator.DocNoDelete(payment.DocumentNo, transaction.GetDbTransaction());

                            await _context.SaveChangesAsync();

                            transaction.Commit();

                        }
                        else {
                            transaction.Rollback();
                            return ApiResult<Response>.ValidationError("Vendor Payment can not be deleted.");

                        }
                    }
                    else{
                        transaction.Rollback();
                        return ApiResult<Response>.ValidationError("Vendor Payment Transaction not found.");
                    }

                    return ApiResult<Response>.Ok(response);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
