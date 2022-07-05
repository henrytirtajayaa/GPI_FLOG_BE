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
using LinqKit;

namespace FLOG_BE.Features.Central.SecurityRoles.GetSecurityRoleDetail
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
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var query = getSecurityRole(request.Filter);
            query = getSecuriryRoleSorted(query, request.Sort);

            var list = await PaginatedList<SecurityRole, ReponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());




            return ApiResult<Response>.Ok(new Response()
            {
                SecurityRoles = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<SecurityRole> getSecurityRole(RequestFilter filter)

        {
            var query = _context.SecurityRoles
                .Include(x => x.RoleAccess)
                .ThenInclude(p => p.Form)
                .OrderBy( x => x.SecurityRoleName)
               .AsQueryable();

            var temp = query.ToList();

            var filterName = filter.RoleId?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<SecurityRole>(true);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.SecurityRoleId.Contains(filterItem));
                }
                query = query.Where(predicate);
            }


            return query;
        }

        private IQueryable<SecurityRole> getSecuriryRoleSorted(IQueryable<SecurityRole> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("RoleName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SecurityRoleName) : query.ThenBy(x => x.SecurityRoleName);
                }

            }

            if (!sortingList.Any(x => x.Contains("RoleName", StringComparison.InvariantCultureIgnoreCase)))
            {
                query = query.ThenBy(x => x.SecurityRoleName);
            }

            return query;
        }
    }
}
