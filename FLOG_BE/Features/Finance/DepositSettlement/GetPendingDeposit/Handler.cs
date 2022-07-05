using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Authentication;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;
using FLOG_BE.Model.Central;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Infrastructure.Utils;
using Entities = FLOG_BE.Model.Companies.Entities;
using LinqKit;
using FLOG_BE.Model.Companies;
using FLOG.Core;
using FLOG_BE.Model.Central.Entities;
using FLOG_BE.Model.Companies.Entities;
using FLOG_BE.Model.Companies.View;

namespace FLOG_BE.Features.Finance.DepositSettlement.GetPendingDeposit
{
    public class Handler : IAsyncRequestHandler<Request, Response>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FlogContext _contextCentral;
        private readonly CompanyContext _context;
        private readonly ILogin _login;
        private readonly HATEOASLinkCollection _linkCollection;

        public Handler(IHttpContextAccessor httpContextAccessor, FlogContext contextCentral, CompanyContext context, ILogin login, HATEOASLinkCollection linkCollection)
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

            var query = getDeposit(request.Initiator.UserId, request.Filter);
            query = getDepositSorted(query, request.Sort);

            var list = await PaginatedList<Model.Companies.View.DepositReceivablePending, Model.Companies.View.DepositReceivablePending>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                DepositReceivable = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<DepositReceivablePending> getDeposit(string personId, RequestFilter filter)
        {
            var query = _context.DepositReceivablePendings.Where(x => x.CustomerId == filter.CustomerId && x.CurrencyCode.ToUpper() == filter.CurrencyCode.ToUpper())
                .AsNoTracking().AsEnumerable().ToList().AsQueryable();

            var wherePredicates = PredicateBuilder.New<DepositReceivablePending>(true);

            if (!string.IsNullOrEmpty(filter.DocumentNo))
            {
                var predicate = PredicateBuilder.New<DepositReceivablePending>(false);
                predicate = predicate.Or(x => x.DocumentNo.ToLower().Contains(filter.DocumentNo.ToLower()));
                wherePredicates.And(predicate);
            }

            if (filter.TransactionDateStart.HasValue)
            {
                var predicate = PredicateBuilder.New<DepositReceivablePending>(false);
                predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date >= filter.TransactionDateStart);
                wherePredicates.And(predicate);
            }

            if (filter.TransactionDateEnd.HasValue)
            {
                var predicate = PredicateBuilder.New<DepositReceivablePending>(false);
                predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date <= filter.TransactionDateEnd);
                wherePredicates.And(predicate);
            }

            if (!string.IsNullOrEmpty(filter.NSDocumentNo))
            {
                var predicate = PredicateBuilder.New<DepositReceivablePending>(false);
                predicate = predicate.Or(x => x.NSDocumentNo.ToLower().Contains(filter.NSDocumentNo));
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);

            return query;
        }

        private IQueryable<DepositReceivablePending> getDepositSorted(IQueryable<DepositReceivablePending> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.TransactionDate);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {

                if (item.Contains("DocumentType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DocumentType) : query.ThenBy(x => x.DocumentType);
                }

                if (item.Contains("DocumentNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DocumentNo) : query.ThenBy(x => x.DocumentNo);
                }

                if (item.Contains("TransactionDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TransactionDate) : query.ThenBy(x => x.TransactionDate);
                }

                if (item.Contains("TransactionType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TransactionType) : query.ThenBy(x => x.TransactionType);
                }

                if (item.Contains("CurrencyCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CurrencyCode) : query.ThenBy(x => x.CurrencyCode);
                }
                if (item.Contains("ExchangeRate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ExchangeRate) : query.ThenBy(x => x.ExchangeRate);
                }


                if (item.Contains("NSDocumentNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.NSDocumentNo) : query.ThenBy(x => x.NSDocumentNo);
                }
                if (item.Contains("Description", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Description) : query.ThenBy(x => x.Description);
                }
                if (item.Contains("OriginatingInvoice", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.OriginatingInvoice) : query.ThenBy(x => x.OriginatingInvoice);
                }
                if (item.Contains("OriginatingPaid", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.OriginatingPaid) : query.ThenBy(x => x.OriginatingPaid);
                }

                if (item.Contains("OriginatingBalance", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.OriginatingBalance) : query.ThenBy(x => x.OriginatingBalance);
                }
            }

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.DocumentNo).ThenBy(x => x.TransactionDate);
            }

            return query;
        }
    }
}
