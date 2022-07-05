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

namespace FLOG_BE.Features.Central.SecurityRoles.DeleteSecurityRole
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
            var CekCompanySecurity = _context.CompanySecurities.FirstOrDefault(x => x.SecurityRoleId == request.Body.RoleId);
            if (CekCompanySecurity == null)
            {
                var record = _context.SecurityRoles
                .FirstOrDefault(x => x.SecurityRoleId == request.Body.RoleId);

                _context.Attach(record);
                _context.Remove(record);

                _context.SecurityRoleAccesses
                 .Where(x => x.SecurityRoleId == request.Body.RoleId).ToList().ForEach(p => _context.Remove(p));

                //REMOVE EXISTING
                _context.SmartRoles
                .Where(x => x.SecurityRoleId == Guid.Parse(request.Body.RoleId)).ToList().ForEach(p => _context.Remove(p));

                await _context.SaveChangesAsync();
                
                return ApiResult<Response>.Ok(new Response()
                {
                    RoleId = request.Body.RoleId
                });

            }
            else
            {
                return ApiResult<Response>.ValidationError("Security Role already in use");
            }
        }
    }
}
