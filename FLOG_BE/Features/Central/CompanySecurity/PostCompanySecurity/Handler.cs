using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Central.Entities;

namespace FLOG_BE.Features.Central.CompanySecurity.PostCompanySecurity
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
            _linkCollection = linkCollection;
            _context = context;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (await _context.CompanySecurities.AnyAsync(x => x.CompanyId == request.Body.CompanyId && x.PersonId == request.Body.UserId))
                return ApiResult<Response>.ValidationError("User name and company already exist.");

            var CompanySecurity = new Entities.CompanySecurity()
            {
                CompanySecurityId = Guid.NewGuid().ToString(),
              
                PersonId = request.Body.UserId,
                CompanyId = request.Body.CompanyId,
                SecurityRoleId = request.Body.RoleId,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now,
            };

            _context.CompanySecurities.Add(CompanySecurity);

            await _context.SaveChangesAsync();

            return ApiResult<Response>.Ok(new Response()
            {
                CompanySecurityId = CompanySecurity.CompanySecurityId
            });
        }
    }
}
