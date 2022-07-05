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

namespace FLOG_BE.Features.Companies.FiscalPeriodDetail.GetFiscalPeriodDetail
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

            var query = getFiscalPeriodDetail(request.Filter);
            query = getFiscalPeriodDetailSorted(query, request.Sort);

            
            var list = await PaginatedList<Entities.FiscalPeriodDetail, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            return ApiResult<Response>.Ok(new Response()
            {
                FiscalPeriodDetails = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.FiscalPeriodDetail> getFiscalPeriodDetail(RequestFilter filter)
        {
            var query = _context.FiscalPeriodDetails.AsQueryable();


            var filterPeriodYear = filter.FiscalHeaderId?.Where(x => x != null).ToList();
            if (filterPeriodYear.Any())
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodDetail>(true);
                foreach (var filterItem in filterPeriodYear)
                {
                    predicate = predicate.Or(x => x.FiscalHeaderId == filterItem);
                }
                query = query.Where(predicate);
            }
            
            var filterPeriodIndex = filter.PeriodIndex?.Where(x => x.HasValue).ToList();
            if (filterPeriodIndex.Any())
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodDetail>(true);
                foreach (var filterItem in filterPeriodIndex)
                {
                    predicate = predicate.Or(x => x.PeriodIndex == filterItem);
                }
                query = query.Where(predicate);
            }
            
            var filterStartDate = filter.PeriodStart?.Where(x => x.HasValue).ToList();
            if (filterStartDate.Any())
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodDetail>(true);
                foreach (var filterItem in filterStartDate)
                {
                    predicate = predicate.Or(x => x.PeriodStart == filterItem);
                }
                query = query.Where(predicate);
            }
            
            var filterEndDate = filter.PeriodEnd?.Where(x => x.HasValue).ToList();
            if (filterEndDate.Any())
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodDetail>(true);
                foreach (var filterItem in filterEndDate)
                {
                    predicate = predicate.Or(x => x.PeriodEnd == filterItem);
                }
                query = query.Where(predicate);
            }

            if (filter.IsClosePurchasing == true)
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodDetail>(true);
                predicate = predicate.Or(x => x.IsClosePurchasing == filter.IsClosePurchasing);
                query = query.Where(predicate);
            }

            if (filter.IsCloseSales == true)
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodDetail>(true);
                predicate = predicate.Or(x => x.IsCloseSales == filter.IsCloseSales);
                query = query.Where(predicate);
            }

            if (filter.IsCloseInventory == true)
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodDetail>(true);
                predicate = predicate.Or(x => x.IsCloseInventory == filter.IsCloseInventory);
                query = query.Where(predicate);
            }

            if (filter.IsCloseFinancial == true)
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodDetail>(true);
                predicate = predicate.Or(x => x.IsCloseFinancial == filter.IsCloseFinancial);
                query = query.Where(predicate);
            }

            if (filter.IsCloseAsset == true)
            {
                var predicate = PredicateBuilder.New<Entities.FiscalPeriodDetail>(true);
                predicate = predicate.Or(x => x.IsCloseAsset == filter.IsCloseAsset);
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.FiscalPeriodDetail> getFiscalPeriodDetailSorted(IQueryable<Entities.FiscalPeriodDetail> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            
            foreach (var item in sortingList)
            {
                if (item.Contains("PeriodStart", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PeriodStart) : query.ThenBy(x => x.PeriodStart);
                }
                if (item.Contains("PeriodEnd", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PeriodEnd) : query.ThenBy(x => x.PeriodEnd);
                }
                if (item.Contains("PeriodIndex", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PeriodIndex) : query.ThenBy(x => x.PeriodIndex);
                }
            }

            if (!sortingList.Any(x => x.Contains("PeriodIndex", StringComparison.InvariantCultureIgnoreCase)))
            {
                query = query.ThenBy(x => x.PeriodIndex);
            }

            return query;
        }
    }
}
