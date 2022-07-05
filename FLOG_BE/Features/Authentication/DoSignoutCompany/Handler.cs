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
using System.Security.Claims;

namespace FLOG_BE.Features.Authentication.DoSignoutCompany
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
            try
            {
                var claims = _httpContextAccessor.HttpContext.User.Claims.ToList();
                if (claims.Count > 0)
                {
                    string personId = claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().Value;
                    string companySecurityId = request.CompanySecurityId; //claims.Where(c=>c.Type == ClaimTypes.Role).FirstOrDefault().Value;

                    if (string.IsNullOrEmpty(companySecurityId))
                        companySecurityId = claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;

                    //Console.WriteLine("[LOGOUT] PersonId " + personId + " | SecId " + companySecurityId + " | companyId " + request.CompanyId);

                    SessionActivityLog newLog = new SessionActivityLog()
                    {
                        ActivityLogId = Guid.NewGuid(),
                        CompanySecurityId = companySecurityId,
                        PersonId = personId,
                        CompanyId = request.CompanyId,
                        Status = DOCSTATUS.INACTIVE,
                        Comments = "LOGOUT",
                        CreatedDate = DateTime.Now
                    };

                    _context.SessionActivityLogs.Add(newLog);

                    if (!string.IsNullOrEmpty(request.CompanyId))
                    {
                        var existingSessions = await _context.SessionStates.Where(x => x.PersonId == personId && x.CompanyId == request.CompanyId).ToListAsync();
                        _context.SessionStates.RemoveRange(existingSessions);

                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(companySecurityId))
                        {
                            var existingSessions = await _context.SessionStates.Where(x => x.PersonId == personId && x.CompanySecurityId == companySecurityId).ToListAsync();
                            _context.SessionStates.RemoveRange(existingSessions);

                            await _context.SaveChangesAsync();
                        }
                    }

                }else {
                    if (!string.IsNullOrEmpty(request.CompanyId))
                    {
                        var existingSessions = await _context.SessionStates.Where(x => x.PersonId == request.PersonId && x.CompanyId == request.CompanyId).ToListAsync();
                        _context.SessionStates.RemoveRange(existingSessions);

                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch(Exception ex)
            {
                return ApiResult<Response>.ValidationError("Sign Out failed ! " + ex.Message);
            }
            
            return ApiResult<Response>.Ok(new Response
            {
                IsSuccess = true,
                Message = "Signout Completed"
            });
        }
    }
}
