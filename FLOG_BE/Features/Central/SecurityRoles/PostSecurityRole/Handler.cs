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

namespace FLOG_BE.Features.Central.SecurityRoles.PostSecurityRole
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

            var Code = _context.SecurityRoles
                .Where(x => x.SecurityRoleCode == request.Body.RoleCode)
                .Select(x => x.SecurityRoleCode).FirstOrDefault();
            
            if (Code != null)
                return ApiResult<Response>.ValidationError("Role Code already exist");

            var SecurityRole = new SecurityRole()
            {
                SecurityRoleId = Guid.NewGuid().ToString(),
                SecurityRoleCode= request.Body.RoleCode,
                SecurityRoleName= request.Body.RoleName,
                CreatedBy = request.Initiator.UserId,
                CreatedDate = DateTime.Now,
               
            };
            
            _context.SecurityRoles.Add(SecurityRole);




            foreach (var item in request.Body.RoleAccess)
            {
                var SecurityRoleAccess = new SecurityRoleAccess()
                {
                    SecurityRoleAccessId = Guid.NewGuid().ToString(),
                    SecurityRoleId = SecurityRole.SecurityRoleId,
                    FormId = item.FormId,
                    CreatedBy = request.Initiator.UserId,
                    CreatedDate = DateTime.Now,
                    AllowNew = item.AllowAccessNew, 
                    AllowOpen = item.AllowAccessOpen,
                    AllowEdit = item.AllowAccessEdit,
                    AllowDelete = item.AllowAccessDelete,
                    AllowPost = item.AllowAccessPost,
                    AllowPrint = item.AllowAccessPrint
                };
                _context.SecurityRoleAccesses.Add(SecurityRoleAccess);
            }
            var validDetail = await InserSmartRole(_context, request.Body, Guid.Parse(SecurityRole.SecurityRoleId));
           
            if(validDetail)
                await _context.SaveChangesAsync();


            return ApiResult<Response>.Ok(new Response()
            {
                RoleId = SecurityRole.SecurityRoleId,
                RoleCode = SecurityRole.SecurityRoleCode,
                RoleName= SecurityRole.SecurityRoleName
            });
        }
        private async Task<bool> InserSmartRole(FlogContext ctx, RequestRole body, Guid SecurityRoleId)
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
