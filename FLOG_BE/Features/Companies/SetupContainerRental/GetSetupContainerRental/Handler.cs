using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Companies;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;
using LinqKit;

namespace FLOG_BE.Features.Companies.SetupContainerRental.GetSetupContainerRental
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
            _login = login;
            _linkCollection = linkCollection;
        }

        public async Task<ApiResult<Response>> Handle(Request request)
        {
            if (string.IsNullOrEmpty(request.Initiator.UserId))
                return ApiResult<Response>.Unauthorized();

            var query = getSetupContainerRental(request.Initiator.UserId, request.Filter);
            query = getSetupContainerRentalSorted(query, request.Sort);

            var list = await PaginatedList<Entities.SetupContainerRental, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            { 
                SetupContainerRentals = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.SetupContainerRental> getSetupContainerRental(string personId, RequestFilter filter)
        {
            var query = _context.SetupContainerRentals.AsQueryable();


            var wherePredicates = PredicateBuilder.New<Entities.SetupContainerRental>(true);

            var filterTransType = filter.TransactionType?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTransType.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SetupContainerRental>(false);
                foreach (var filterItem in filterTransType)
                {
                    predicate = predicate.Or(x => x.TransactionType.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            var filterRequest = filter.RequestDocNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterRequest.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SetupContainerRental>(false);
                foreach (var filterItem in filterRequest)
                {
                    predicate = predicate.Or(x => x.RequestDocNo.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            var filterdelivery = filter.DeliveryDocNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterdelivery.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SetupContainerRental>(false);
                foreach (var filterItem in filterdelivery)
                {
                    predicate = predicate.Or(x => x.DeliveryDocNo.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            var filterclosing = filter.ClosingDocNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterclosing.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SetupContainerRental>(false);
                foreach (var filterItem in filterclosing)
                {
                    predicate = predicate.Or(x => x.ClosingDocNo.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            var filteruom = filter.UomScheduleCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filteruom.Any())
            {
                var predicate = PredicateBuilder.New<Entities.SetupContainerRental>(false);
                foreach (var filterItem in filteruom)
                {
                    predicate = predicate.Or(x => x.UomScheduleCode.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            if (filter.CustomerFreeUsageDays.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.SetupContainerRental>(false);
                predicate = predicate.Or(x => x.CustomerFreeUsageDays == filter.CustomerFreeUsageDays);
                wherePredicates.And(predicate);
            }

            if (filter.ShippingLineFreeUsageDays.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.SetupContainerRental>(false);
                predicate = predicate.Or(x => x.ShippingLineFreeUsageDays == filter.ShippingLineFreeUsageDays);
                wherePredicates.And(predicate);
            }

            if (filter.CntOwnerFreeUsageDays.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.SetupContainerRental>(false);
                predicate = predicate.Or(x => x.CntOwnerFreeUsageDays == filter.CntOwnerFreeUsageDays);
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);

            return query;
        }

        private IQueryable<Entities.SetupContainerRental> getSetupContainerRentalSorted(IQueryable<Entities.SetupContainerRental> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("TransactionType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TransactionType) : query.ThenBy(x => x.TransactionType);
                }
                if (item.Contains("RequestDocNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.RequestDocNo) : query.ThenBy(x => x.RequestDocNo);
                }
                if (item.Contains("DeliveryDocNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DeliveryDocNo) : query.ThenBy(x => x.DeliveryDocNo);
                }
                if (item.Contains("ClosingDocNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ClosingDocNo) : query.ThenBy(x => x.ClosingDocNo);
                }
            }

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.TransactionType);
            }

            return query;
        }
    }
}
