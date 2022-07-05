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

namespace FLOG_BE.Features.Companies.PaymentTerm.PostPaymentTerm
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

            if (await _context.PaymentTerms.AnyAsync(x => x.PaymentTermCode == request.Body.PaymentTermCode))
                return ApiResult<Response>.ValidationError("Payment Term Code already exist.");
            

            var payment = new Entities.PaymentTerm()
            {
                PaymentTermId = Guid.NewGuid(),
                PaymentTermCode = request.Body.PaymentTermCode,
                PaymentTermDesc = request.Body.PaymentTermDesc,
                Due = request.Body.Due,
                Unit = request.Body.Unit,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now
            };

            _context.PaymentTerms.Add(payment);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                PaymentTermId = payment.PaymentTermId.ToString(),
                PaymentTermCode = payment.PaymentTermCode,
                PaymentTermDesc = payment.PaymentTermDesc
            });
        }
    }
}
