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

namespace FLOG_BE.Features.Companies.TaxRefferenceNumber.GetTaxRefferenceNumber
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _linkCollection = linkCollection;
            _login = login;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var query = getTaxRefferenceNumber(request.Initiator.UserId, request.Filter);
            query = getTaxRefferenceNumberSorted(query, request.Sort);

            var list = await PaginatedList<Entities.TaxRefferenceNumber, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            { 
                TaxRefferenceNumbers = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.TaxRefferenceNumber> getTaxRefferenceNumber(string personId, RequestFilter filter)
        {
            var query = _context.TaxRefferenceNumbers.AsQueryable();

            var filterStartDateStart = filter.StartDateStart?.Where(x => x.HasValue).ToList();
            if (filterStartDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxRefferenceNumber>(true);
                foreach (var filterItem in filterStartDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.StartDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }
            var filterStartDateEnd = filter.StartDateEnd?.Where(x => x.HasValue).ToList();
            if (filterStartDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxRefferenceNumber>(true);
                foreach (var filterItem in filterStartDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.StartDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterReffNoStart = filter.ReffNoStartMin?.Where(x => x.HasValue).ToList();
            if (filterReffNoStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxRefferenceNumber>(true);
                foreach (var filterItem in filterReffNoStart)
                {
                    predicate = predicate.Or(x => x.ReffNoStart >= filterItem);
                }
                query = query.Where(predicate);
            }
            var filterReffNoStartMax = filter.ReffNoStartMax?.Where(x => x.HasValue).ToList();
            if (filterReffNoStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxRefferenceNumber>(true);
                foreach (var filterItem in filterReffNoStartMax)
                {
                    predicate = predicate.Or(x => x.ReffNoStart <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterReffNoEnd = filter.ReffNoEndMin?.Where(x => x.HasValue).ToList();
            if (filterReffNoEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxRefferenceNumber>(true);
                foreach (var filterItem in filterReffNoEnd)
                {
                    predicate = predicate.Or(x => x.ReffNoEnd >= filterItem);
                }
                query = query.Where(predicate);
            }
            var filterReffNoEndMax = filter.ReffNoEndMax?.Where(x => x.HasValue).ToList();
            if (filterReffNoEndMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxRefferenceNumber>(true);
                foreach (var filterItem in filterReffNoEndMax)
                {
                    predicate = predicate.Or(x => x.ReffNoEnd <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterDocLengthMin = filter.DocLengthMin?.Where(x => x.HasValue).ToList();
            if (filterDocLengthMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxRefferenceNumber>(true);
                foreach (var filterItem in filterDocLengthMin)
                {
                    predicate = predicate.Or(x => x.DocLength >= filterItem);
                }
                query = query.Where(predicate);
            }
            var filterDocLengthMax = filter.DocLengthMax?.Where(x => x.HasValue).ToList();
            if (filterDocLengthMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxRefferenceNumber>(true);
                foreach (var filterItem in filterDocLengthMax)
                {
                    predicate = predicate.Or(x => x.DocLength <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterLastNoMin = filter.LastNoMin?.Where(x => x.HasValue).ToList();
            if (filterLastNoMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxRefferenceNumber>(true);
                foreach (var filterItem in filterLastNoMin)
                {
                    predicate = predicate.Or(x => x.LastNo >= filterItem);
                }
                query = query.Where(predicate);
            }
            var filterLastNoMax = filter.LastNoMax?.Where(x => x.HasValue).ToList();
            if (filterLastNoMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxRefferenceNumber>(true);
                foreach (var filterItem in filterLastNoMax)
                {
                    predicate = predicate.Or(x => x.LastNo <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterStatus = filter.Status?.Where(x => x.HasValue).ToList();
            if (filterStatus.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxRefferenceNumber>(true);
                foreach (var filterItem in filterStatus)
                {
                    predicate = predicate.Or(x => x.Status == filterItem);
                }
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxRefferenceNumber>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.ToLower().Contains(filterItem.ToLower()));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxRefferenceNumber>(true);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxRefferenceNumber>(true);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxRefferenceNumber>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.ToLower().Contains(filterItem.ToLower()));
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxRefferenceNumber>(true);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxRefferenceNumber>(true);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.TaxRefferenceNumber> getTaxRefferenceNumberSorted(IQueryable<Entities.TaxRefferenceNumber> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("StartDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.StartDate) : query.ThenBy(x => x.StartDate);
                }

                if (item.Contains("ReffNoStart", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ReffNoStart) : query.ThenBy(x => x.ReffNoStart);
                }

                if (item.Contains("ReffNoEnd", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ReffNoEnd) : query.ThenBy(x => x.ReffNoEnd);
                }

                if (item.Contains("DocLength", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DocLength) : query.ThenBy(x => x.DocLength);
                }

                if (item.Contains("LastNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.LastNo) : query.ThenBy(x => x.LastNo);
                }

                if (item.Contains("Status", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Status) : query.ThenBy(x => x.Status);
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
                query = query.ThenBy(x => x.StartDate);
            }

            return query;
        }
    }
}
