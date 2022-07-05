using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using FLOG_BE.Model.Companies;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;
using LinqKit;

namespace FLOG_BE.Features.Companies.Container.GetContainer
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _contextCentral;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, FlogContext contextCentral, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _contextCentral = contextCentral;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var query = getContainer(request.Filter);
            query = getContainerSorted(query, request.Sort);
            List<Entities.Reference> ListRef = await _context.References.ToListAsync();

            var list = await PaginatedList<Entities.Container, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            List<ResponseItem> response;

            response = new List<ResponseItem>(list.Select(x => new ResponseItem
            {
             
                ContainerId = x.ContainerId,
                ContainerCode = x.ContainerCode,
                ContainerName = x.ContainerName,
                ContainerSize = x.ContainerSize,
                ContainerType = x.ContainerType,
                ContainerTeus = x.ContainerTeus,
                Inactive = x.Inactive,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedDate = x.ModifiedDate,
            })); ;


            return ApiResult<Response>.Ok(new Response()
            {
                Containers = response,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.Container> getContainer(RequestFilter filter)
        {
            var query = _context.Containers.AsQueryable();

            var filterCode = filter.ContainerCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Container>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.ContainerCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterName = filter.ContainerName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Container>(true);
                foreach (var filterItem in filterName)
                {
                    predicate = predicate.Or(x => x.ContainerName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterSizeMin = filter.ContainerSizeMin?.Where(x => x.HasValue).ToList();
            if (filterSizeMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Container>(true);
                foreach (var filterItem in filterSizeMin)
                {
                    predicate = predicate.Or(x => x.ContainerSize >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterSizeMax = filter.ContainerSizeMax?.Where(x => x.HasValue).ToList();
            if (filterSizeMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Container>(true);
                foreach (var filterItem in filterSizeMax)
                {
                    predicate = predicate.Or(x => x.ContainerSize <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterRefType = filter.ContainerType?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterRefType.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Container>(true);
                foreach (var filterItem in filterRefType)
                {
                    predicate = predicate.Or(x => x.ContainerType.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterTeusMin = filter.ContainerTeusMin?.Where(x => x.HasValue).ToList();
            if (filterTeusMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Container>(true);
                foreach (var filterItem in filterTeusMin)
                {
                    predicate = predicate.Or(x => x.ContainerTeus >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterTeusMax = filter.ContainerTeusMax?.Where(x => x.HasValue).ToList();
            if (filterTeusMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Container>(true);
                foreach (var filterItem in filterTeusMax)
                {
                    predicate = predicate.Or(x => x.ContainerTeus <= filterItem);
                }
                query = query.Where(predicate);
            }

            if (filter.Inactive.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Container>(true);
                predicate = predicate.Or(x => x.Inactive == filter.Inactive);
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Container>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.ToLower().Contains(filterItem.ToLower()));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Container>(true);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Container>(true);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Container>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.ToLower().Contains(filterItem.ToLower()));
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Container>(true);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Container>(true);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.Container> getContainerSorted(IQueryable<Entities.Container> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("ContainerCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ContainerCode) : query.ThenBy(x => x.ContainerCode);
                }
                if (item.Contains("ContainerName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ContainerName) : query.ThenBy(x => x.ContainerName);
                }
                if (item.Contains("ContainerSize", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ContainerSize) : query.ThenBy(x => x.ContainerSize);
                }
                if (item.Contains("ContainerType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ContainerType) : query.ThenBy(x => x.ContainerType);
                }
                if (item.Contains("ContainerTeus", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ContainerTeus) : query.ThenBy(x => x.ContainerTeus);
                }
                if (item.Contains("Inactive", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Inactive) : query.ThenBy(x => x.Inactive);
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

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.ContainerCode);
            }

            return query;
        }
    }
}
