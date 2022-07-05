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
using Entities = FLOG_BE.Model.Central.Entities;
using LinqKit;
using FLOG_BE.Model.Central.Entities;

namespace FLOG_BE.Features.Central.CompanySecurity.GetCompanySecurity
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

            var query = getUserCompany(request.Filter);
            query = getUserCompanySorted(query, request.Sort);
            List<Person> ListUser = await GetUser();

            var list = await PaginatedList<Entities.CompanySecurity, ReponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            List<ReponseItem> responseCompanySecurity;

            responseCompanySecurity = new List<ReponseItem>(list.Select(x => new ReponseItem
            {
            
                CompanySecurityId = x.CompanySecurityId,
                UserId = x.UserId,
                CompanyId = x.CompanyId,
                CompanyName = x.CompanyName,
                RoleId = x.RoleId,
                UserName = x.UserName,
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
                CompanySecurities = responseCompanySecurity,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.CompanySecurity> getUserCompany(RequestFilter filter)
        {
            var query = _context.CompanySecurities
                .Include(x => x.Company)
                .Include(x => x.Person)
                .Include(x => x.SecurityRole)
                .AsQueryable();

            var filterPerson = filter.UserName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterPerson.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanySecurity>(true);
                foreach (var filterItem in filterPerson)
                {
                    predicate = predicate.Or(x => x.Person.PersonFullName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCompany = filter.CompanyName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCompany.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanySecurity>(true);
                foreach (var filterItem in filterCompany)
                {
                    predicate = predicate.Or(x => x.Company.CompanyName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterRole = filter.RoleName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterRole.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanySecurity>(true);
                foreach (var filterItem in filterRole)
                {
                    predicate = predicate.Or(x => x.SecurityRole.SecurityRoleName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanySecurity>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }
            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanySecurity>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanySecurity>(true);

                foreach (DateTime filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date >= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanySecurity>(true);

                foreach (DateTime filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.And(x => ((DateTime)x.CreatedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanySecurity>(true);

                foreach (DateTime filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date >= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.CompanySecurity>(true);

                foreach (DateTime filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.And(x => ((DateTime)x.ModifiedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.CompanySecurity> getUserCompanySorted(IQueryable<Entities.CompanySecurity> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("UserName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Person.PersonFullName) : query.ThenBy(x => x.Person.PersonFullName);
                }

                if (item.Contains("CompanyName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Company.CompanyName) : query.ThenBy(x => x.Company.CompanyName);
                }
                if (item.Contains("RoleName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SecurityRole.SecurityRoleName) : query.ThenBy(x => x.SecurityRole.SecurityRoleName);
                }
            }

            if (!sortingList.Any(x => x.Contains("UserName", StringComparison.InvariantCultureIgnoreCase)))
            {
                query = query.ThenBy(x => x.Person.PersonFullName);
            }

            return query;
        }
        private async Task<List<Person>> GetUser()
        {
            return await _context.Persons.ToListAsync();
        }
    }
}
