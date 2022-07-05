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

namespace FLOG_BE.Features.Central.SecurityRoles.GetSecurityRole
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
            List<Person> ListUser = await GetUser();

            var list = await PaginatedList<SecurityRole, ReponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            List<ReponseItem> responseSecurityRole;

            responseSecurityRole = new List<ReponseItem>(list.Select(x => new ReponseItem
            {

                RoleId = x.RoleId,
                RoleCode = x.RoleCode,
                RoleName = x.RoleName,
                CreatedBy = x.CreatedBy,
                CreatedByName = ListUser.Where(p => p.PersonId == x.CreatedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedByName = ListUser.Where(p => p.PersonId == x.ModifiedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                ModifiedDate = x.ModifiedDate,
            })); 




            return ApiResult<Response>.Ok(new Response()
            {
                SecurityRoles = responseSecurityRole,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<SecurityRole> getSecurityRole(RequestFilter filter)
        {
            var query = _context.SecurityRoles
               .AsQueryable();


            var filterCode = filter.RoleCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<SecurityRole>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.SecurityRoleCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterName = filter.RoleName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<SecurityRole>(true);
                foreach (var filterItem in filterName)
                {

                    if (string.IsNullOrEmpty(filterItem))
                        predicate = predicate.Or(x => x.SecurityRoleName == "");
                    else
                        predicate = predicate.Or(x => x.SecurityRoleName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }
            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<SecurityRole>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }
            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<SecurityRole>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<SecurityRole>(true);

                foreach (DateTime filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date >= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<SecurityRole>(true);

                foreach (DateTime filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.And(x => ((DateTime)x.CreatedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<SecurityRole>(true);

                foreach (DateTime filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date >= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<SecurityRole>(true);

                foreach (DateTime filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.And(x => ((DateTime)x.ModifiedDate).Date <= filterItem);
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
                if (item.Contains("RoleCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SecurityRoleCode) : query.ThenBy(x => x.SecurityRoleCode);
                }

                if (item.Contains("RoleName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SecurityRoleName) : query.ThenBy(x => x.SecurityRoleName);
                }

                if (item.Contains("CreatedBy", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CreatedBy) : query.ThenBy(x => x.CreatedBy);
                }
                if (item.Contains("CreatedDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CreatedDate) : query.ThenBy(x => x.CreatedDate);
                }

                if (item.Contains("ModifiedBy", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ModifiedBy) : query.ThenBy(x => x.ModifiedBy);
                }
                if (item.Contains("ModifiedDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ModifiedDate) : query.ThenBy(x => x.ModifiedDate);
                }

            }

            if (!sortingList.Any(x => x.Contains("RoleName", StringComparison.InvariantCultureIgnoreCase)))
            {
                query = query.ThenBy(x => x.SecurityRoleName);
            }

            return query;
        }

        private  async Task<List<Person>> GetUser()
        {
            return await _context.Persons.ToListAsync();
        }
    }
}
