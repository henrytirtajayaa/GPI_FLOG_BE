using FLOG_BE.Model.Central;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Central.CompanySecurity.DeleteCompanySecurity
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;


        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _login = login;
            _linkCollection = linkCollection;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            var record = _context.CompanySecurities.FirstOrDefault(x => x.CompanySecurityId == request.Body.CompanySecurityId);

            if (record == null)
            {
                return ApiResult<Response>.ValidationError($"{nameof(request.Body.CompanySecurityId)} Not Found!");
            }
            else {
                _context.Attach(record);
                _context.Remove(record);
                await _context.SaveChangesAsync();

                return ApiResult<Response>.Ok(new Response()
                { 
                    CompanySecurityId = request.Body.CompanySecurityId
                });
            }
        }
    }
}
