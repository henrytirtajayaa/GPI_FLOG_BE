using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FLOG_BE.Features.Authentication.DoLogin
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
            var usr = _context.Persons.FirstOrDefault(x => x.EmailAddress == request.Email);
            if (usr != null)
            {
                if (_login.PasswordIsMatch(request.Password, usr.PersonPassword))
                {
                    var tokenString = _login.CreateToken(UserClaim.PreLogin, usr);
                    return ApiResult<Response>.Ok(new Response
                    {
                        IsSuccess = true,
                        DetailUser = new DetailUser
                        {
                            UserId = usr.PersonId,
                            EmailAddress = usr.EmailAddress,
                            UserGroupId = usr.PersonCategoryId,
                            UserName = usr.PersonFullName
                        },
                        AccessToken = tokenString,
                        UserRoles = await getUserCompanyRoles(usr.PersonId)
                    }); ;
                }
                else
                {
                    return ApiResult<Response>.ValidationError("Incorrect Password!");
                }
            }
            else
            {
                return ApiResult<Response>.ValidationError("User Not Found!");
            }
        }

        private async Task<List<UserRole>> getUserCompanyRoles(string personId)
        {
            return await _context.CompanySecurities
                     .Include(x => x.Company)
                     .Include(x => x.SecurityRole)
                     .Where(x => x.PersonId == personId)
                     .Select(x => new UserRole()
                     {
                         SecurityId = x.CompanySecurityId,
                         CompanyName = x.Company.CompanyName,
                         RoleName = x.SecurityRole.SecurityRoleName
                     }).ToListAsync();
        }
    }
}
