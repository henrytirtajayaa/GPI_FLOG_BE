using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FLOG_BE.Model.Central;
using FLOG_BE.Model.Central.Entities;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using LinqKit;

namespace FLOG_BE.Features.Central.UserGroup.GetUserGroup
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

            var query = getUserGroup(request.Initiator.UserId, request.Filter);
            query = getUserGroupSorted(query, request.Sort);

            var list = await PaginatedList<PersonCategory, ReponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                UserGroups = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<PersonCategory> getUserGroup(string personId, RequestFilter filter)
        {
            var query = _context.PersonCategories.AsQueryable();

            var filterCode = filter.UserGroupCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<PersonCategory>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.PersonCategoryCode.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterName = filter.UserGroupName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<PersonCategory>(true);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.PersonCategoryName.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<PersonCategory>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<PersonCategory>(true);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<PersonCategory>(true);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterUpdatedBy = filter.UpdatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterUpdatedBy.Any())
            {
                var predicate = PredicateBuilder.New<PersonCategory>(true);
                foreach (var filterItem in filterUpdatedBy)
                {
                    predicate = predicate.Or(x => x.UpdatedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterUpdatedDateStart = filter.UpdatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterUpdatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<PersonCategory>(true);
                foreach (var filterItem in filterUpdatedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.UpdatedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterUpdatedDateEnd = filter.UpdatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterUpdatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<PersonCategory>(true);
                foreach (var filterItem in filterUpdatedDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.UpdatedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<PersonCategory> getUserGroupSorted(IQueryable<PersonCategory> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("UserGroupCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PersonCategoryCode) : query.ThenBy(x => x.PersonCategoryCode);
                }

                if (item.Contains("UserGroupName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PersonCategoryName) : query.ThenBy(x => x.PersonCategoryName);
                }

                if (item.Contains("CreatedBy", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CreatedBy) : query.ThenBy(x => x.CreatedBy);
                }

                if (item.Contains("CreatedDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CreatedDate) : query.ThenBy(x => x.CreatedDate);
                }

                if (item.Contains("UpdatedBy", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.UpdatedBy) : query.ThenBy(x => x.UpdatedBy);
                }

                if (item.Contains("UpdatedDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.UpdatedDate) : query.ThenBy(x => x.UpdatedDate);
                }
            }

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.PersonCategoryCode);
            }

            return query;
        }
    }
}
