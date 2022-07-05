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
using AutoMapper;

namespace FLOG_BE.Features.Companies.ExchangeRateDetail.GetExchangeRateDetail
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _contextCentral;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;
        private readonly IMapper _mapper;

        public Handler(IHttpContextAccessor httpContextAccessor, CompanyContext context, FlogContext contextCentral, ILogin login, HATEOASLinkCollection linkCollection, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _linkCollection = linkCollection;
            _context = context;
            _contextCentral = contextCentral;
            _login = login;
            _mapper = mapper;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var query = getExchangeRateDetail(request.Filter);
            query = getExchangeRateSorted(query, request.Sort);

            var items = query.ToList();

            var response = new Response();
            response.GetExchangeRateDetail = new List<ResponseItem>();

            foreach (var item in items)
            {
                response.GetExchangeRateDetail.Add(new ResponseItem
                {
                    ExchangeRateDetailId = item.ExchangeRateDetailId,
                    ExchangeRateHeaderId = item.ExchangeRateHeaderId,
                    ExpiredDate = item.ExpiredDate,
                    RateDate = item.RateDate,
                    RateAmount = item.RateAmount,
                    Status = item.Status
                });
            }

            return ApiResult<Response>.Ok(new Response()
            {
                GetExchangeRateDetail = response.GetExchangeRateDetail,
                ListInfo = null
            });
        }

        private IQueryable<Entities.ExchangeRateDetail> getExchangeRateDetail(RequestFilter filter)
        {
            IQueryable<Entities.ExchangeRateDetail> query;
            if (filter.ExchangeRateHeaderId != Guid.Empty)
            {
                query = _context.ExchangeRateDetails.Where(x => x.ExchangeRateHeaderId.Equals(filter.ExchangeRateHeaderId)).AsQueryable();
            }
            else
            {
                query = _context.ExchangeRateDetails.AsQueryable();
            }

            var filterExpiredDateStart = filter.ExpiredDateStart?.Where(x => x.HasValue).ToList();
            if (filterExpiredDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateDetail>(true);
                foreach (var filterItem in filterExpiredDateStart)
                {
                    predicate = predicate.Or(x => x.ExpiredDate >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterExpiredDateEnd = filter.ExpiredDateEnd?.Where(x => x.HasValue).ToList();
            if (filterExpiredDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateDetail>(true);
                foreach (var filterItem in filterExpiredDateEnd)
                {
                    predicate = predicate.Or(x => x.ExpiredDate <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterRateDateStart = filter.RateDateStart?.Where(x => x.HasValue).ToList();
            if (filterRateDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateDetail>(true);
                foreach (var filterItem in filterRateDateStart)
                {
                    predicate = predicate.Or(x => x.RateDate >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterRateDateEnd = filter.RateDateEnd?.Where(x => x.HasValue).ToList();
            if (filterRateDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateDetail>(true);
                foreach (var filterItem in filterRateDateEnd)
                {
                    predicate = predicate.Or(x => x.RateDate <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterStatusMin = filter.StatusMin?.Where(x => x.HasValue).ToList();
            if (filterStatusMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateDetail>(true);
                foreach (var filterItem in filterStatusMin)
                {
                    predicate = predicate.Or(x => x.Status >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterStatusMax = filter.StatusMax?.Where(x => x.HasValue).ToList();
            if (filterStatusMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateDetail>(true);
                foreach (var filterItem in filterStatusMax)
                {
                    predicate = predicate.Or(x => x.Status <= filterItem);
                }
                query = query.Where(predicate);
            }


            var filterRateAmountMin = filter.RateAmountMin?.Where(x => x.HasValue).ToList();
            if (filterRateAmountMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateDetail>(true);
                foreach (var filterItem in filterRateAmountMin)
                {
                    predicate = predicate.Or(x => x.RateAmount >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterRateAmountMax = filter.RateAmountMax?.Where(x => x.HasValue).ToList();
            if (filterRateAmountMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ExchangeRateDetail>(true);
                foreach (var filterItem in filterRateAmountMax)
                {
                    predicate = predicate.Or(x => x.RateAmount <= filterItem);
                }
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.ExchangeRateDetail> getExchangeRateSorted(IQueryable<Entities.ExchangeRateDetail> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());

            foreach (var item in sortingList)
            {
                if (item.Contains("RateDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.RateDate) : query.ThenBy(x => x.RateDate);
                }

                if (item.Contains("ExpiredDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ExpiredDate) : query.ThenBy(x => x.ExpiredDate);
                }
                if (item.Contains("RateAmount", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.RateAmount) : query.ThenBy(x => x.RateAmount);
                }
                if (item.Contains("Status", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Status) : query.ThenBy(x => x.Status);
                }
            }

            if (!sortingList.Any(x => x.Contains("RateDate", StringComparison.InvariantCultureIgnoreCase)))
            {
                query = query.ThenBy(x => x.RateDate);
            }

            return query;
        }


    }
}