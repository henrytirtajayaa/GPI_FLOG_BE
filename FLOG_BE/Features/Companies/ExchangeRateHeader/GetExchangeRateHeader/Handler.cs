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

namespace FLOG_BE.Features.Companies.ExchangeRateHeader.GetExchangeRateHeader
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

            var query = getExchangeRateHeader(request.Filter);
            query = getExchangeRateHeaderSorted(query, request.Sort);
            List<Person> ListUser = await GetUser();

            var list = await PaginatedList<Entities.ExchangeRateHeader, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            List<ResponseItem> responseExchangeRate;

            responseExchangeRate = new List<ResponseItem>(list.Select(x => new ResponseItem
            {
                ExchangeRateHeaderId = x.ExchangeRateHeaderId,
                ExchangeRateCode = x.ExchangeRateCode,
                Description = x.Description,
                CurrencyCode = x.CurrencyCode,
                RateType = x.RateType,
                ExpiredPeriod = x.ExpiredPeriod,
                CalculationType = x.CalculationType,
                Status = x.Status,
                CreatedBy = x.CreatedBy,
                CreatedByName = ListUser.Where(p => p.PersonId == x.CreatedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                CreatedDate = x.CreatedDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedByName = ListUser.Where(p => p.PersonId == x.ModifiedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                ModifiedDate = x.ModifiedDate,
            }));


            return ApiResult<Response>.Ok(new Response()
            {
                ExchangeRateHeaders = responseExchangeRate,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.ExchangeRateHeader> getExchangeRateHeader(RequestFilter filter)
        {

            var query = _context.ExchangeRateHeaders.AsQueryable();

            var filterCode = filter.ExchangeRateCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateHeader>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.ExchangeRateCode.Contains(filterItem));
                }
                query = query.Where(predicate);
            }
            var filterDescription = filter.Description?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDescription.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateHeader>(true);
                foreach (var filterItem in filterDescription)
                {
                    predicate = predicate.Or(x => x.Description.Contains(filterItem));
                }
                query = query.Where(predicate);
            }
            var filterCurrency = filter.CurrencyCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCurrency.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateHeader>(true);
                foreach (var filterItem in filterCurrency)
                {
                    predicate = predicate.Or(x => x.CurrencyCode.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterRateMin = filter.RateTypeMin?.Where(x => x.HasValue).ToList();
            if (filterRateMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateHeader>(true);
                foreach (var filterItem in filterRateMin)
                {
                    predicate = predicate.Or(x => x.RateType >= filterItem);
                }
                query = query.Where(predicate);
            }
            var filterRateMax = filter.RateTypeMax?.Where(x => x.HasValue).ToList();
            if (filterRateMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateHeader>(true);
                foreach (var filterItem in filterRateMax)
                {
                    predicate = predicate.Or(x => x.RateType <= filterItem);
                }
                query = query.Where(predicate);
            }


            var filterExpire = filter.ExpiredPeriod?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterExpire.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateHeader>(true);
                foreach (var filterItem in filterExpire)
                {
                    predicate = predicate.Or(x => x.ExpiredPeriod.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterCalcMin = filter.CalculationTypeMin?.Where(x => x.HasValue).ToList();
            if (filterCalcMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateHeader>(true);
                foreach (var filterItem in filterCalcMin)
                {
                    predicate = predicate.Or(x => x.CalculationType >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterCalcMax = filter.CalculationTypeMax?.Where(x => x.HasValue).ToList();
            if (filterCalcMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateHeader>(true);
                foreach (var filterItem in filterCalcMax)
                {
                    predicate = predicate.Or(x => x.CalculationType <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterStatusMin = filter.StatusMin?.Where(x => x.HasValue).ToList();
            if (filterStatusMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateHeader>(true);
                foreach (var filterItem in filterStatusMin)
                {
                    predicate = predicate.Or(x => x.Status >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterStatusMax = filter.StatusMax?.Where(x => x.HasValue).ToList();
            if (filterStatusMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateHeader>(true);
                foreach (var filterItem in filterStatusMax)
                {
                    predicate = predicate.Or(x => x.Status <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateHeader>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }
            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateHeader>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateHeader>(true);

                foreach (DateTime filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date >= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateHeader>(true);

                foreach (DateTime filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.And(x => ((DateTime)x.CreatedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateHeader>(true);

                foreach (DateTime filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date >= filterItem);
                }

                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateHeader>(true);

                foreach (DateTime filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.And(x => ((DateTime)x.ModifiedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }



            return query;
        }

        private IQueryable<Entities.ExchangeRateHeader> getExchangeRateHeaderSorted(IQueryable<Entities.ExchangeRateHeader> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {

                if (item.Contains("ExchangeRateCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ExchangeRateCode) : query.ThenBy(x => x.ExchangeRateCode);
                }
                if (item.Contains("Description", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Description) : query.ThenBy(x => x.Description);
                }
                if (item.Contains("CurrencyCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CurrencyCode) : query.ThenBy(x => x.CurrencyCode);
                }
                if (item.Contains("RateType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.RateType) : query.ThenBy(x => x.RateType);
                }
                if (item.Contains("ExpiredPeriod", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ExpiredPeriod) : query.ThenBy(x => x.ExpiredPeriod);
                }
                if (item.Contains("CalculationType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CalculationType) : query.ThenBy(x => x.CalculationType);
                }
                if (item.Contains("Status", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Status) : query.ThenBy(x => x.Status);
                }

            }

            return query;
        }
        private async Task<List<Person>> GetUser()
        {
            return await _contextCentral.Persons.ToListAsync();
        }
    }
}
