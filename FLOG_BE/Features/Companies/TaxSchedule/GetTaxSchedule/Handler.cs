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

namespace FLOG_BE.Features.Companies.TaxSchedule.GetTaxSchedule
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

            var query = getTaxSchedule(request.Filter);
            query = getTaxScheduleSorted(query, request.Sort);


            var list = await PaginatedList<Entities.TaxSchedule, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            return ApiResult<Response>.Ok(new Response()
            {
                TaxSchedules = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.TaxSchedule> getTaxSchedule(RequestFilter filter)
        {
            var query = _context.TaxSchedules.AsQueryable();

            var filterId = filter.TaxScheduleId;
            if (filterId.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                predicate = predicate.Or(x => x.TaxScheduleId.Equals(filterId));
                query = query.Where(predicate);
            }

            var filterCode = filter.TaxScheduleCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in filterCode)
                {
                    predicate = predicate.Or(x => x.TaxScheduleCode.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterDescription = filter.Description?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDescription.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in filterDescription)
                {
                    predicate = predicate.Or(x => x.Description.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterTaxAccountNo = filter.TaxAccountNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTaxAccountNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in filterTaxAccountNo)
                {
                    predicate = predicate.Or(x => x.TaxAccountNo.Contains(filterItem));
                }
                query = query.Where(predicate);
            }

            var filterIsSales = filter.IsSales?.Where(x => x.HasValue).ToList();
            if (filterIsSales.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in filterIsSales)
                {
                    predicate = predicate.Or(x => x.IsSales == filterItem);
                }
                query = query.Where(predicate);
            }


            var filterPercentOfSalesPurchaseMin = filter.PercentOfSalesPurchaseMin?.Where(x => x.HasValue).ToList();
            if (filterPercentOfSalesPurchaseMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in filterPercentOfSalesPurchaseMin)
                {
                    predicate = predicate.Or(x => x.PercentOfSalesPurchase >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterPercentOfSalesPurchaseMax = filter.PercentOfSalesPurchaseMax?.Where(x => x.HasValue).ToList();
            if (filterPercentOfSalesPurchaseMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in filterPercentOfSalesPurchaseMax)
                {
                    predicate = predicate.Or(x => x.PercentOfSalesPurchase <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterTaxablePercentMin = filter.TaxablePercentMin?.Where(x => x.HasValue).ToList();
            if (filterTaxablePercentMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in filterTaxablePercentMin)
                {
                    predicate = predicate.Or(x => x.TaxablePercent >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterTaxablePercentMax = filter.TaxablePercentMax?.Where(x => x.HasValue).ToList();
            if (filterTaxablePercentMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in filterTaxablePercentMax)
                {
                    predicate = predicate.Or(x => x.TaxablePercent <= filterItem);
                }
                query = query.Where(predicate);
            }

            var FilterRoundingTypeMin = filter.RoundingType?.Where(x => x.HasValue).ToList();
            if (FilterRoundingTypeMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in FilterRoundingTypeMin)
                {
                    predicate = predicate.Or(x => x.RoundingType == filterItem);
                }
                query = query.Where(predicate);
            }

           
            var FilterRoundingLimitAmountMin = filter.RoundingLimitAmountMin?.Where(x => x.HasValue).ToList();
            if (FilterRoundingLimitAmountMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in FilterRoundingLimitAmountMin)
                {
                    predicate = predicate.Or(x => x.RoundingLimitAmount >= filterItem);
                }
                query = query.Where(predicate);
            }

            var FilterRoundingLimitAmountMax = filter.RoundingLimitAmountMax?.Where(x => x.HasValue).ToList();
            if (FilterRoundingLimitAmountMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in FilterRoundingLimitAmountMax)
                {
                    predicate = predicate.Or(x => x.RoundingLimitAmount <= filterItem);
                }
                query = query.Where(predicate);
            }

            var FilterTaxInclude = filter.TaxInclude?.Where(x => x.HasValue).ToList();
            if (FilterTaxInclude.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in FilterTaxInclude)
                {
                    predicate = predicate.Or(x => x.TaxInclude == filterItem);
                }
                query = query.Where(predicate);
            }

            var FilterWithHoldingTax = filter.WithHoldingTax?.Where(x => x.HasValue).ToList();
            if (FilterWithHoldingTax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in FilterWithHoldingTax)
                {
                    predicate = predicate.Or(x => x.WithHoldingTax == filterItem);
                }
                query = query.Where(predicate);
            }

            var FilterInactive = filter.Inactive?.Where(x => x.HasValue).ToList();
            if (FilterInactive.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in FilterInactive)
                {
                    predicate = predicate.Or(x => x.Inactive == filterItem);
                }
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.ToLower().Contains(filterItem.ToLower()));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.ToLower().Contains(filterItem.ToLower()));
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.TaxSchedule>(true);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date <= filterItem);
                }

                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.TaxSchedule> getTaxScheduleSorted(IQueryable<Entities.TaxSchedule> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("TaxScheduleCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TaxScheduleCode) : query.ThenBy(x => x.TaxScheduleCode);
                }
                if (item.Contains("Description", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Description) : query.ThenBy(x => x.Description);
                }
                if (item.Contains("IsSales", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.IsSales) : query.ThenBy(x => x.IsSales);
                }
                if (item.Contains("PercentOfSalesPurchase", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PercentOfSalesPurchase) : query.ThenBy(x => x.PercentOfSalesPurchase);
                }
                if (item.Contains("TaxablePercent", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TaxablePercent) : query.ThenBy(x => x.TaxablePercent);
                }
                if (item.Contains("RoundingType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.RoundingType) : query.ThenBy(x => x.RoundingType);
                }
                if (item.Contains("RoundingLimitAmount", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.RoundingLimitAmount) : query.ThenBy(x => x.RoundingLimitAmount);
                }
                if (item.Contains("TaxInclude", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TaxInclude) : query.ThenBy(x => x.TaxInclude);
                }
                if (item.Contains("WithHoldingTax", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.WithHoldingTax) : query.ThenBy(x => x.WithHoldingTax);
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
                query = query.ThenBy(x => x.TaxScheduleCode);
            }

            return query;
        }
    }
}
