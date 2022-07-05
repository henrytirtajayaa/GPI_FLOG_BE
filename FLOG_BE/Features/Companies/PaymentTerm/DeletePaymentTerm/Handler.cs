using System;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Companies;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;

namespace FLOG_BE.Features.Companies.PaymentTerm.DeletePaymentTerm
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
            _context = context;
            _login = login;
            _linkCollection = linkCollection;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            var record = _context.PaymentTerms.FirstOrDefault(x => x.PaymentTermId == Guid.Parse(request.Body.PaymentTermId));

            if (record == null)
            {
                return ApiResult<Response>.ValidationError("PaymentTerm Not Found");
            } else {
                _context.Attach(record);
                _context.Remove(record);
                await _context.SaveChangesAsync();

                return ApiResult<Response>.Ok(new Response()
                { 
                    PaymentTermId = request.Body.PaymentTermId
                });
            };
        }
    }
}
