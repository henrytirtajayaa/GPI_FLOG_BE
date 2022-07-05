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
using FLOG_BE.Model.Central.Entities;

namespace FLOG_BE.Features.Central.CompanySecurity.PutCompanySecurity
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
            var response = new Response()
            {
                CompanySecurityId = request.Body.CompanySecurityId,
                CompanyId = request.Body.CompanyId,
                UserId = request.Body.UserId,
                SecurityRoleId = request.Body.RoleId
            };

            var companySecurity = await _context.CompanySecurities.FirstOrDefaultAsync(x => x.CompanySecurityId == request.Body.CompanySecurityId);
            if (companySecurity != null)
            {
                companySecurity.CompanyId = request.Body.CompanyId;
                companySecurity.SecurityRoleId = request.Body.RoleId;
                companySecurity.PersonId = request.Body.UserId;
                companySecurity.ModifiedBy = request.Initiator.UserId;
                companySecurity.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                response.CompanyId = companySecurity.CompanyId;
                response.SecurityRoleId = companySecurity.SecurityRoleId;
                response.UserId = companySecurity.PersonId;
            }

            return ApiResult<Response>.Ok(response);
        }
    }
}
