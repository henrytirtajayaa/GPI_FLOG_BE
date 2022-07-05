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

namespace FLOG_BE.Features.Companies.Currency.GetCurrency
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

            var query = getCurrency(request.Filter);
            query = getCurrencySorted(query, request.Sort);

            
            var list = await PaginatedList<Entities.Currency, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            return ApiResult<Response>.Ok(new Response()
            {
                Currencies = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.Currency> getCurrency(RequestFilter filter)
        {
            var query = _context.Currencies.AsQueryable();
            
            var financialSetup = _context.FinancialSetups.FirstOrDefault();
            if(financialSetup!= null)
            {
                var funcCurrency = query.Where(x => x.CurrencyCode.ToUpper().Equals(financialSetup.FuncCurrencyCode)).FirstOrDefault();

                if (funcCurrency != null)
                    funcCurrency.IsFunctional = true;
            }

            var filterCurrencyCode = filter.CurrencyCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCurrencyCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                foreach (var filterItem in filterCurrencyCode)
                {
                    predicate = predicate.Or(x => x.CurrencyCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterDescription = filter.Description?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDescription.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                foreach (var filterItem in filterDescription)
                {
                    predicate = predicate.Or(x => x.Description.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCurrencyUnit = filter.CurrencyUnit?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCurrencyUnit.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                foreach (var filterItem in filterCurrencyUnit)
                {
                    predicate = predicate.Or(x => x.CurrencyUnit.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCurrencySubUnit = filter.CurrencySubUnit?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCurrencySubUnit.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                foreach (var filterItem in filterCurrencySubUnit)
                {
                    predicate = predicate.Or(x => x.CurrencySubUnit.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }


            var filterSymbol = filter.Symbol?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSymbol.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                foreach (var filterItem in filterSymbol)
                {
                    predicate = predicate.Or(x => x.Symbol.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }
            
            var filterDecimalPlacesMin = filter.DecimalPlacesMin?.Where(x => x.HasValue).ToList();
            if (filterDecimalPlacesMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                foreach (var filterItem in filterDecimalPlacesMin)
                {
                    predicate = predicate.Or(x => x.DecimalPlaces >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterDecimalPlacesMax = filter.DecimalPlacesMax?.Where(x => x.HasValue).ToList();
            if (filterDecimalPlacesMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                foreach (var filterItem in filterDecimalPlacesMax)
                {
                    predicate = predicate.Or(x => x.DecimalPlaces <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterRealizedGainAcc = filter.RealizedGainAcc?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterRealizedGainAcc.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                foreach (var filterItem in filterRealizedGainAcc)
                {
                    predicate = predicate.Or(x => x.RealizedGainAcc.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterRealizedLossAcc = filter.RealizedLossAcc?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterRealizedLossAcc.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                foreach (var filterItem in filterRealizedLossAcc)
                {
                    predicate = predicate.Or(x => x.RealizedLossAcc.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterUnrealizedGainAcc = filter.UnrealizedGainAcc?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterUnrealizedGainAcc.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                foreach (var filterItem in filterUnrealizedGainAcc)
                {
                    predicate = predicate.Or(x => x.UnrealizedGainAcc.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterUnrealizedLossAcc = filter.UnrealizedLossAcc?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterUnrealizedLossAcc.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                foreach (var filterItem in filterUnrealizedLossAcc)
                {
                    predicate = predicate.Or(x => x.UnrealizedLossAcc.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            if (filter.Inactive.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                predicate = predicate.Or(x => x.Inactive == filter.Inactive);
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.Currency>(true);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.Currency> getCurrencySorted(IQueryable<Entities.Currency> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("CurrencyCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CurrencyCode) : query.ThenBy(x => x.CurrencyCode);
                }
                if (item.Contains("Description", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Description) : query.ThenBy(x => x.Description);
                }
                if (item.Contains("Symbol", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Symbol) : query.ThenBy(x => x.Symbol);
                }
                if (item.Contains("DecimalPlaces", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DecimalPlaces) : query.ThenBy(x => x.DecimalPlaces);
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
                query = query.ThenBy(x => x.CurrencyCode);
            }

            return query;
        }
    }
}
