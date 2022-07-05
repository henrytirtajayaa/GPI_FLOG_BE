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

namespace FLOG_BE.Features.Central.SecurityRoles.PutSecurityRole
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
                RoleId = request.Body.RoleId,
                RoleCode = request.Body.RoleCode,
                RoleName = request.Body.RoleName,

            };

            var securityRole = await _context.SecurityRoleAccesses.FirstOrDefaultAsync(x => x.SecurityRoleId == request.Body.RoleId);

            if (securityRole != null)
            {
                
                 _context.SecurityRoleAccesses.Where(x => x.SecurityRoleId == request.Body.RoleId)
                    .ToList().ForEach(x => _context.SecurityRoleAccesses.Remove(x));

                _context.SmartRoles.Where(x => x.SecurityRoleId == Guid.Parse(request.Body.RoleId))
                   .ToList().ForEach(x => _context.SmartRoles.Remove(x));
            }
           
            foreach (var item in request.Body.RoleAccess)
                {
                    var SecurityRoleAccess = new SecurityRoleAccess()
                    {
                        SecurityRoleAccessId = Guid.NewGuid().ToString(),
                        SecurityRoleId = request.Body.RoleId,
                        FormId = item.FormId,
                        ModifiedBy = request.Initiator.UserId,
                        ModifiedDate = DateTime.Now,
                        AllowNew = item.AllowAccessNew,
                        AllowOpen = item.AllowAccessOpen,
                        AllowEdit = item.AllowAccessEdit,
                        AllowDelete = item.AllowAccessDelete,
                        AllowPost = item.AllowAccessPost,
                        AllowPrint = item.AllowAccessPrint

                    };
                    _context.SecurityRoleAccesses.Add(SecurityRoleAccess);
                }
            var validDetail = await InserSmartRole(_context, request.Body, Guid.Parse(request.Body.RoleId));

            if (validDetail)
                await _context.SaveChangesAsync();
            
            return ApiResult<Response>.Ok(response);
        }
        private async Task<bool> InserSmartRole(FlogContext ctx, RequestBodyRoleUpdate body, Guid SecurityRoleId)
        {
            if (body.RoleSmart != null)
            {
                foreach (var item in body.RoleSmart)
                {
                    var detail = new Model.Central.Entities.SmartRole()
                    {
                        Id = Guid.NewGuid(),
                        SecurityRoleId = SecurityRoleId,
                        SmartviewId = Guid.Parse(item.SmartviewId),

                    };
                    //_context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.ar_receipt_detail ON");
                    _context.SmartRoles.Add(detail);
                }
                await ctx.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
