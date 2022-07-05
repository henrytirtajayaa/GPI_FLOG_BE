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

namespace FLOG_BE.Features.Companies.MSTransactionType.GetTransactionType
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

            var query = getTransactionType(request.Initiator.UserId, request.Filter);
            query = getTransactionTypeSorted(query, request.Sort);

            var list = await PaginatedList<Entities.MSTransactionType, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                TransactionTypes = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<Entities.MSTransactionType> getTransactionType(string personId, RequestFilter filter)
        {
            var query = _context.MSTransactionTypes.Where(x=>x.InActive == false).AsQueryable();

            var filterTransactionType = filter.TransactionType?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTransactionType.Any())
            {
                var predicate = PredicateBuilder.New<Entities.MSTransactionType>(true);
                foreach (var filterItem in filterTransactionType)
                {
                    predicate = predicate.Or(x => x.TransactionType.ToUpper().Contains(filterItem.ToUpper()));
                }
                query = query.Where(predicate);
            }

            var filterTransactionName = filter.TransactionName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTransactionName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.MSTransactionType>(true);
                foreach (var filterItem in filterTransactionName)
                {
                    predicate = predicate.Or(x => x.TransactionName.ToUpper().Contains(filterItem.ToUpper()));
                }
                query = query.Where(predicate);
            }

            var filterPaymentCondition = filter.PaymentCondition?.Where(x => x.HasValue).ToList();
            if (filterPaymentCondition.Any())
            {
                var predicate = PredicateBuilder.New<Entities.MSTransactionType>(true);
                foreach (var filterItem in filterPaymentCondition)
                {
                    predicate = predicate.Or(x => x.PaymentCondition == filterItem);
                }
                query = query.Where(predicate);
            }

            var filterRequiredSubject = filter.RequiredSubject?.Where(x => x.HasValue).ToList();
            if (filterRequiredSubject.Any())
            {
                var predicate = PredicateBuilder.New<Entities.MSTransactionType>(true);
                foreach (var filterItem in filterRequiredSubject)
                {
                    predicate = predicate.Or(x => x.RequiredSubject == filterItem);
                }
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<Entities.MSTransactionType> getTransactionTypeSorted(IQueryable<Entities.MSTransactionType> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("TransactionType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TransactionType) : query.ThenBy(x => x.TransactionType);
                }
                if (item.Contains("TransactionName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TransactionName) : query.ThenBy(x => x.TransactionName);
                }
                if (item.Contains("PaymentCondition", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PaymentCondition) : query.ThenBy(x => x.PaymentCondition);
                }
                if (item.Contains("RequiredSubject", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.RequiredSubject) : query.ThenBy(x => x.RequiredSubject);
                }

                if (item.Contains("InActive", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.InActive) : query.ThenBy(x => x.InActive);
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
