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
using FLOG_BE.Model.Central.Entities;

namespace FLOG_BE.Features.Companies.FiscalPeriodHeader.GetFiscalPeriodHeader
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

            var query = getFiscalPeriodHeader(request.Filter);
            query = getFiscalPeriodHeaderSorted(query, request.Sort);
            List<Person> ListUser = await GetUser();

            var list = await PaginatedList<Entities.FiscalPeriodHeader, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            List<ResponseItem> responseFiscalPeriod;

            responseFiscalPeriod = new List<ResponseItem>(list.Select(x => new ResponseItem
            {
                FiscalHeaderId = x.FiscalHeaderId,
                PeriodYear = x.PeriodYear,
                TotalPeriod = x.TotalPeriod,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                ClosingYear = x.ClosingYear,
                CreatedBy = x.CreatedBy,
                CreatedByName = ListUser.Where(p => p.PersonId == x.CreatedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedByName = ListUser.Where(p => p.PersonId == x.ModifiedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                ModifiedDate = x.ModifiedDate,
            })); ;


            return ApiResult<Response>.Ok(new Response()
            {
                FiscalPeriodHeaders = responseFiscalPeriod,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.FiscalPeriodHeader> getFiscalPeriodHeader(RequestFilter filter)
        {
            var query = _context.FiscalPeriodHeaders.AsQueryable();


            var filterPeriodYear = filter.PeriodYearMin?.Where(x => x.HasValue).ToList();
            if (filterPeriodYear.Any())
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodHeader>(true);
                foreach (var filterItem in filterPeriodYear)
                {
                    predicate = predicate.Or(x => x.PeriodYear >= filterItem);
                }
                query = query.Where(predicate);
            }
            var filterPeriodYearMax = filter.PeriodYearMax?.Where(x => x.HasValue).ToList();
            if (filterPeriodYearMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodHeader>(true);
                foreach (var filterItem in filterPeriodYearMax)
                {
                    predicate = predicate.Or(x => x.PeriodYear <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterTotalPeriodMin = filter.TotalPeriodMin?.Where(x => x.HasValue).ToList();
            if (filterTotalPeriodMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodHeader>(true);
                foreach (var filterItem in filterPeriodYear)
                {
                    predicate = predicate.Or(x => x.TotalPeriod == filterItem);
                }
                query = query.Where(predicate);
            }

            var filterStartDate = filter.StartDateStart?.Where(x => x.HasValue).ToList();
            if (filterStartDate.Any())
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodHeader>(true);
                foreach (var filterItem in filterStartDate)
                {
                    predicate = predicate.Or(x => x.StartDate >= filterItem);
                }
                query = query.Where(predicate);
            }
            var filterStartDateEnd = filter.StartDateEnd?.Where(x => x.HasValue).ToList();
            if (filterStartDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodHeader>(true);
                foreach (var filterItem in filterStartDateEnd)
                {
                    predicate = predicate.Or(x => x.StartDate <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterEndDateStart = filter.EndDateStart?.Where(x => x.HasValue).ToList();
            if (filterEndDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodHeader>(true);
                foreach (var filterItem in filterEndDateStart)
                {
                    predicate = predicate.Or(x => x.EndDate >= filterItem);
                }

                query = query.Where(predicate);
            }
            var filterEndDateEnd = filter.EndDateEnd?.Where(x => x.HasValue).ToList();
            if (filterEndDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodHeader>(true);
                foreach (var filterItem in filterEndDateStart)
                {
                    predicate = predicate.Or(x => x.EndDate <= filterItem);
                }

                query = query.Where(predicate);
            }


            if (filter.ClosingYear.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodHeader>(true);
                predicate = predicate.Or(x => x.ClosingYear == filter.ClosingYear);
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodHeader>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }
            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodHeader>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodHeader>(true);

                foreach (DateTime filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date >= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodHeader>(true);

                foreach (DateTime filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.And(x => ((DateTime)x.CreatedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodHeader>(true);

                foreach (DateTime filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date >= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodHeader>(true);

                foreach (DateTime filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.And(x => ((DateTime)x.ModifiedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }


            return query;
        }

        private IQueryable<Entities.FiscalPeriodHeader> getFiscalPeriodHeaderSorted(IQueryable<Entities.FiscalPeriodHeader> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.PeriodYear);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());

            foreach (var item in sortingList)
            {
                if (item.Contains("PeriodYear", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PeriodYear) : query.ThenBy(x => x.PeriodYear);
                }
                if (item.Contains("TotalPeriod", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TotalPeriod) : query.ThenBy(x => x.TotalPeriod);
                }
                if (item.Contains("StartDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.StartDate) : query.ThenBy(x => x.StartDate);
                }
                if (item.Contains("EndDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.EndDate) : query.ThenBy(x => x.EndDate);
                }

                if (item.Contains("ClosingYear", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ClosingYear) : query.ThenBy(x => x.ClosingYear);
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

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.PeriodYear);
            }

            return query;
        }
        private async Task<List<Person>> GetUser()
        {
            return await _contextCentral.Persons.ToListAsync();
        }
    }
}
