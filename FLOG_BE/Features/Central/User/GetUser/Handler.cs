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
using FLOG_BE.Features.Authentication.GetTokenDetail;
using AutoMapper;

namespace FLOG_BE.Features.Central.User.GetUser
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

            var query = getUser(request.Filter);
            query = getUserSorted(query, request.Sort);
             

            var list = await PaginatedList<Entities.Person, ReponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            List<ReponseItem> responseUsers;

            responseUsers = new List<ReponseItem>(list.Select(x => new ReponseItem
            {
                UserId = x.UserId,
                UserFullName = x.UserFullName,
                UserPassword = x.UserPassword,
                UserGroupId = x.UserGroupId,
                UserGroupCode = x.UserGroupCode == null ? "" : x.UserGroupCode.ToString(),
                EmailAddress = x.EmailAddress,
                InActive = x.InActive,
                CreatedBy = x.CreatedBy,
                CreatedByName =  list.Where(p => p.UserId == x.CreatedBy).Select( p => p.UserFullName).FirstOrDefault(),
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedByName = list.Where(p => p.UserId == x.ModifiedBy).Select(p => p.UserFullName).FirstOrDefault(),
                ModifiedDate = x.ModifiedDate,
            })); ;

            return ApiResult<Response>.Ok(new Response()
            {

                Users = responseUsers,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.Person> getUser(RequestFilter filter)
        {
            var query = _context.Persons
                .Include(x => x.PersonCategory)
                .AsQueryable();

            var filterCode = filter.UserGroupCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Person>(false);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.PersonCategory.PersonCategoryCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }
            var filterName = filter.UserFullName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Person>(false);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.PersonFullName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }
            var filterCategory = filter.UserGroupCode?.Where(x => string.IsNullOrEmpty(x)).ToList();
            if (filterCategory.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Person>(true);
                foreach (var filterItem in filterCategory)
                {
                    if (string.IsNullOrEmpty(filterItem))
                        predicate = predicate.Or(x => x.PersonCategoryId == "");
                    else
                        predicate = predicate.Or(x => x.PersonCategoryId.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterEmail = filter.EmailAddress?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterEmail.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Person>(true);
                foreach (var filterItem in filterEmail)
                {
                    predicate = predicate.Or(x => x.EmailAddress.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            if (filter.InActive.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Person>(true);
                predicate = predicate.Or(x => x.InActive == filter.InActive);
                query = query.Where(predicate);
            }


            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Person>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }
            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Person>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Person>(true);

                foreach (DateTime filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date >= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Person>(true);

                foreach (DateTime filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.And(x => ((DateTime)x.CreatedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Person>(true);

                foreach (DateTime filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date >= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Person>(true);

                foreach (DateTime filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.And(x => ((DateTime)x.ModifiedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }


            return query;
        }

        private IQueryable<Entities.Person> getUserSorted(IQueryable<Entities.Person> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("UserFullName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PersonFullName) : query.ThenBy(x => x.PersonFullName);
                }
                if (item.Contains("EmailAddress", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.EmailAddress) : query.ThenBy(x => x.EmailAddress);
                }

                if (item.Contains("UserGroupCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PersonCategory.PersonCategoryCode) : query.ThenBy(x => x.PersonCategory.PersonCategoryCode);
                }
                if (item.Contains("InActive", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.InActive) : query.ThenBy(x => x.InActive);
                }
                if (item.Contains("CreatedBy", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CreatedBy) : query.ThenBy(x => x.CreatedBy);
                }
                if (item.Contains("ModifiedBy", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ModifiedBy) : query.ThenBy(x => x.ModifiedBy);
                }
                if (item.Contains("CreatedDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CreatedDate) : query.ThenBy(x => x.CreatedDate);
                }
                 if (item.Contains("ModifiedDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ModifiedDate) : query.ThenBy(x => x.ModifiedDate);
                }

            }

            if (!sortingList.Any(x => x.Contains("UserFullName", StringComparison.InvariantCultureIgnoreCase)))
            {
                query = query.ThenBy(x => x.PersonFullName);
            }

            return query;
        }

        
    }
}
