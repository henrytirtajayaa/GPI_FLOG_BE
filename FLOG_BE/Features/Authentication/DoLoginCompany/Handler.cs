using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using FLOG_BE.Model.Central.Entities;
using FLOG_BE.Helper;
using FLOG.Core;

namespace FLOG_BE.Features.Authentication.DoLoginCompany
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _context;
        private readonly ILogin _login;

        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext context, ILogin login)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
         
            var usr = _context.Persons.FirstOrDefault(x => x.PersonId == request.UserId);
            if (usr != null)
            {
                var companySecurity = await _context.CompanySecurities.FirstOrDefaultAsync(x => x.PersonId == request.UserId && x.CompanySecurityId == request.RoleId);
                if (companySecurity != null)
                {
                    //BEGIN : Remove Previous Session State
                    if (request.CompanyId != "undefined" && request.CompanyId != null)
                    {
                        var existingSessions = await _context.SessionStates.Where(x => x.PersonId == usr.PersonId && x.CompanyId == request.CompanyId).ToListAsync();
                        _context.SessionStates.RemoveRange(existingSessions);

                    }else {
                        var existingSessions = await _context.SessionStates.Where(x => x.PersonId == usr.PersonId && x.CompanySecurityId == request.RoleId).ToListAsync();
                        _context.SessionStates.RemoveRange(existingSessions);


                    }


                    await _context.SaveChangesAsync();
                    //END : Remove Previous Session State

                    //BEGIN : Add New Session State
                    SessionState newSess = new SessionState()
                    {
                        Id = Guid.NewGuid(), CompanySecurityId = companySecurity.CompanySecurityId,
                        PersonId = usr.PersonId, CompanyId = companySecurity.CompanyId, 
                        Status = DOCSTATUS.NEW, CreatedDate = DateTime.Now
                    };
                    
                    _context.SessionStates.Add(newSess);

                    SessionActivityLog newLog = new SessionActivityLog()
                    {
                        ActivityLogId = Guid.NewGuid(),
                        CompanySecurityId = newSess.CompanySecurityId,
                        PersonId = newSess.PersonId,
                        CompanyId = newSess.CompanyId,
                        Status = DOCSTATUS.NEW,
                        Comments = "LOGIN",
                        CreatedDate = DateTime.Now
                    };

                    _context.SessionActivityLogs.Add(newLog);

                    await _context.SaveChangesAsync();

                    //END : Add New Session State

                    var tokenString = _login.CreateTokenWithCompany(UserClaim.Company, usr, companySecurity);
                    return ApiResult<Response>.Ok(new Response
                    {
                        IsSuccess = true,
                        Token = tokenString,
                        CompanyId = companySecurity.CompanyId,
                        CompanySecurityId = companySecurity.CompanySecurityId,
                        Menus = await AuthenticationHelper.GetMenus(_context, companySecurity.SecurityRoleId),
                        SessionId = newSess.Id,
                    });
                }
                else
                {
                    return ApiResult<Response>.ValidationError("User Access Not Found!");
                }
            }
            else
            {
                return ApiResult<Response>.ValidationError("User Not Found!");
            }
        }
    }
}
