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

namespace FLOG_BE.Features.Companies.PaymentTerm.PutPaymentTerm
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {

            var response = new Response()
            {
                PaymentTermId = request.Body.PaymentTermId.ToString(),
                PaymentTermCode = request.Body.PaymentTermCode,
                PaymentTermDesc = request.Body.PaymentTermDesc
            };

            var payment = await _context.PaymentTerms.FirstOrDefaultAsync(x => x.PaymentTermId == Guid.Parse(request.Body.PaymentTermId));
            if (payment != null)
            {
                payment.PaymentTermCode = request.Body.PaymentTermCode;
                payment.PaymentTermDesc = request.Body.PaymentTermDesc;
                payment.Due = request.Body.Due;
                payment.Unit = request.Body.Unit;
                payment.ModifiedBy = request.Initiator.UserId;
                payment.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                response.PaymentTermCode = payment.PaymentTermCode;
            }else {
                return ApiResult<Response>.ValidationError("PaymentTerm not found.");
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
