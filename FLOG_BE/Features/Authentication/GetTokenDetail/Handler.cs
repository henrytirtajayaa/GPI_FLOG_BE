using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AutoMapper;
using FLOG_BE.Helper;
using FLOG_BE.Features.Authentication.DoLogin;

namespace FLOG_BE.Features.Authentication.GetTokenDetail
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _context;
        private readonly ILogin _login;
        private readonly IMapper _mapper;

        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext context, ILogin login, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _login = login;
            _mapper = mapper;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            var routePath = "";
            int end= request.Route.Length;
            routePath = request.Route.Substring(end - 1) == "/" ? request.Route.Remove(end - 1) : request.Route;

            var FormId = await _context.Forms.Where(x => x.FormLink.Equals(routePath)).Select(x => x.FormId).FirstOrDefaultAsync();

            Console.WriteLine("*************** ROUTE ***************" + routePath);

            if (!routePath.ToLower().Equals("/login") && !routePath.Contains("nofpage",StringComparison.OrdinalIgnoreCase))
            {
                if (!await _context.Forms.AnyAsync(x => x.FormLink.Equals(routePath)))
                {
                    return ApiResult<Response>.NotFound(null);
                }
            }
            
            if(!routePath.ToLower().Equals("/login") && !routePath.Contains("nofpage", StringComparison.OrdinalIgnoreCase))
            {
                if (!await _context.SecurityRoleAccesses
                .Include(x => x.Form)
                .AnyAsync(x => x.SecurityRoleId == request.Initiator.SecurityId && x.Form != null && x.Form.FormLink.Equals(routePath)))
                {
                    return ApiResult<Response>.NotAcceptable(null);
                }
            }           

            var usr = _context.Persons.FirstOrDefault(x => x.PersonId == request.Initiator.UserId);
            if (usr != null)
            {
                return ApiResult<Response>.Ok(new Response
                {
                    IsSuccess = true,
                    DetailUser = new ResponsePerson
                    {
                        UserId = usr.PersonId,
                        EmailAddress = usr.EmailAddress,
                        UserGroupId = usr.PersonCategoryId,
                        UserName = usr.PersonFullName,
                       
                        
                    },
                    Company = await getCompany(request.Initiator.CompanyId),
                    Role = await getRole(request.Initiator.SecurityId),
                    ListRole = await getUserCompanyRoles(request.Initiator.UserId),
                    Menus = await AuthenticationHelper.GetMenus(_context, request.Initiator.SecurityId),
                    RoleAccess = await getRoleAccess(request.Initiator.SecurityId,FormId),
                    CompanySecurityId =  getCompanySecurity(request.Initiator.UserId, request.Initiator.CompanyId),
                    SessionId = getSessionId(request.Initiator.UserId),
                });
            }
            else
            {
                return ApiResult<Response>.ValidationError("User Not Found!");
            }
        }

        private async Task<ResponseCompany> getCompany(string companyId)
        {
            return _mapper.Map<ResponseCompany>(await _context.Companies.FirstOrDefaultAsync(x => x.CompanyId == companyId));
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
                         CompanyId = x.Company.CompanyId,
                         CompanyName = x.Company.CompanyName,
                         RoleName = x.SecurityRole.SecurityRoleName
                        
                     }).ToListAsync();
        }
        private async Task<ResponseRole> getRole(string roleId)
        {
            return _mapper.Map<ResponseRole>(await _context.SecurityRoles.FirstOrDefaultAsync(x => x.SecurityRoleId == roleId));
        }
        private async Task<ResponseRoleAccess> getRoleAccess(string SecurityRoleId,string FormId)
        {
            var roleAllowAccess = await _context.SecurityRoleAccesses.FirstOrDefaultAsync(x => x.SecurityRoleId == SecurityRoleId && x.FormId == FormId);

            if (roleAllowAccess == null)
            {
                roleAllowAccess = new Model.Central.Entities.SecurityRoleAccess();
                roleAllowAccess.AllowDelete = false;
                roleAllowAccess.AllowEdit = false;
                roleAllowAccess.AllowNew = false;
                roleAllowAccess.AllowOpen = false;
                roleAllowAccess.AllowPost = false;
                roleAllowAccess.AllowPrint = false;
            }

            return _mapper.Map<ResponseRoleAccess>(roleAllowAccess);
        }
        private  string getCompanySecurity(string userId,string CompanyId)
        {
           return  _context.CompanySecurities.Where(x => x.PersonId == userId && x.CompanyId == CompanyId).Select( x => x.CompanySecurityId).FirstOrDefault();

        }
        private  string getSessionId(string userId)
        {
           return  _context.SessionStates.Where(x => x.PersonId == userId ).Select( x => x.Id.ToString()).FirstOrDefault();

        }
    }
}
