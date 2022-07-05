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

namespace FLOG_BE.Features.Companies.Checkbook.GetCheckbook
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

            var query = getCheckbook(request.Initiator.UserId, request.Filter);
            query = getCheckbookSorted(query, request.Sort);

            var list = await PaginatedList<ResponseItem, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                Checkbooks = list,
                ListInfo = list.ListInfo
            });
        }

        private IQueryable<ResponseItem> getCheckbook(string personId, RequestFilter filter)
        {

            var query = (from u in _context.Checkbooks
                             join curr in _context.Currencies on u.CurrencyCode equals curr.CurrencyCode
                             select new
                             {
                                 CheckbookId = u.CheckbookId,
                                 CheckbookCode = u.CheckbookCode,
                                 BankName = (string.IsNullOrEmpty(u.BankCode) ? (_context.Banks.Where(b=>b.BankCode.Equals(u.BankCode, StringComparison.OrdinalIgnoreCase)).Select(z=>z.BankName).FirstOrDefault()) : ""),
                                 CheckbookName = u.CheckbookName,
                                 CurrencyCode = u.CurrencyCode,
                                 BankCode = u.BankCode,
                                 BankAccountCode = u.BankAccountCode,
                                 CheckbookAccountNo = u.CheckbookAccountNo,
                                 HasCheckoutApproval = u.HasCheckoutApproval,
                                 ApprovalCode = u.ApprovalCode,
                                 CheckbookInDocNo = u.CheckbookInDocNo,
                                 CheckbookOutDocNo = u.CheckbookOutDocNo,
                                 ReceiptDocNo = u.ReceiptDocNo,
                                 PaymentDocNo = u.PaymentDocNo,
                                 ReconcileDocNo = u.ReconcileDocNo,
                                 IsCash = u.IsCash,
                                 InActive = u.InActive,
                                 DecimalPlaces = curr.DecimalPlaces
                            }).AsEnumerable().Select(u => new ResponseItem()
                             {
                                 CheckbookId = u.CheckbookId.ToString(),
                                 CheckbookCode = u.CheckbookCode,
                                 BankName = u.BankName,
                                 CheckbookName = u.CheckbookName,
                                 CurrencyCode = u.CurrencyCode,
                                 BankCode = u.BankCode,
                                 BankAccountCode = u.BankAccountCode,
                                 CheckbookAccountNo = u.CheckbookAccountNo,
                                 HasCheckoutApproval = u.HasCheckoutApproval,
                                 ApprovalCode = u.ApprovalCode,
                                CheckbookInDocNo = u.CheckbookInDocNo,
                                CheckbookOutDocNo = u.CheckbookOutDocNo,
                                ReceiptDocNo = u.ReceiptDocNo,
                                PaymentDocNo = u.PaymentDocNo,
                                ReconcileDocNo = u.ReconcileDocNo,
                                IsCash = u.IsCash,
                                InActive = u.InActive,
                                DecimalPlaces = u.DecimalPlaces
                            }).AsQueryable().Distinct();

            var filterCheckbookCode = filter.CheckbookCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCheckbookCode.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterCheckbookCode)
                {
                    predicate = predicate.Or(x => x.CheckbookCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCheckbookName = filter.CheckbookName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCheckbookName.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterCheckbookName)
                {
                    predicate = predicate.Or(x => x.CheckbookName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCheckbookAccountNo = filter.CheckbookAccountNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCheckbookAccountNo.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterCheckbookAccountNo)
                {
                    predicate = predicate.Or(x => x.CheckbookAccountNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterBankAccountCode = filter.BankAccountCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterBankAccountCode.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterBankAccountCode)
                {
                    predicate = predicate.Or(x => x.BankAccountCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCurrencyCode = filter.CurrencyCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCurrencyCode.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterCurrencyCode)
                {
                    predicate = predicate.Or(x => x.CurrencyCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterBankCode = filter.BankCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterBankCode.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterBankCode)
                {
                    predicate = predicate.Or(x => x.BankCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            if (filter.HasCheckoutApproval.HasValue)
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                predicate = predicate.Or(x => x.HasCheckoutApproval == filter.HasCheckoutApproval);
                query = query.Where(predicate);
            }

            var filterApprovalCode = filter.ApprovalCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterApprovalCode.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterApprovalCode)
                {
                    predicate = predicate.Or(x => x.ApprovalCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCheckbookInDocNo = filter.CheckbookInDocNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCheckbookInDocNo.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterCheckbookInDocNo)
                {
                    predicate = predicate.Or(x => x.CheckbookInDocNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCheckbookOutDocNo = filter.CheckbookOutDocNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCheckbookOutDocNo.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterCheckbookOutDocNo)
                {
                    predicate = predicate.Or(x => x.CheckbookOutDocNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterReceiptDocNo = filter.ReceiptDocNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterReceiptDocNo.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterReceiptDocNo)
                {
                    predicate = predicate.Or(x => x.ReceiptDocNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterPaymentDocNo = filter.PaymentDocNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterPaymentDocNo.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterPaymentDocNo)
                {
                    predicate = predicate.Or(x => x.PaymentDocNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterReconcileDocNo = filter.ReconcileDocNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterReconcileDocNo.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterReconcileDocNo)
                {
                    predicate = predicate.Or(x => x.ReconcileDocNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            if (filter.IsCash.HasValue)
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                predicate = predicate.Or(x => x.IsCash == filter.IsCash);
                query = query.Where(predicate);
            }

            if (filter.InActive.HasValue)
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                predicate = predicate.Or(x => x.InActive == filter.InActive);
                query = query.Where(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                query = query.Where(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<ResponseItem> getCheckbookSorted(IQueryable<ResponseItem> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("CheckbookCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CheckbookCode) : query.ThenBy(x => x.CheckbookCode);
                }

                if (item.Contains("CheckbookName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CheckbookName) : query.ThenBy(x => x.CheckbookName);
                }

                if (item.Contains("BankAccountCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.BankAccountCode) : query.ThenBy(x => x.BankAccountCode);
                }

                if (item.Contains("CheckbookAccountNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CheckbookAccountNo) : query.ThenBy(x => x.CheckbookAccountNo);
                }

                if (item.Contains("CurrencyCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CurrencyCode) : query.ThenBy(x => x.CurrencyCode);
                }

                if (item.Contains("BankCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.BankCode) : query.ThenBy(x => x.BankCode);
                }

                if (item.Contains("HasCheckoutApproval", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.HasCheckoutApproval) : query.ThenBy(x => x.HasCheckoutApproval);
                }

                if (item.Contains("ApprovalCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ApprovalCode) : query.ThenBy(x => x.ApprovalCode);
                }

                if (item.Contains("CheckbookInDocNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CheckbookInDocNo) : query.ThenBy(x => x.CheckbookInDocNo);
                }

                if (item.Contains("CheckbookOutDocNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CheckbookOutDocNo) : query.ThenBy(x => x.CheckbookOutDocNo);
                }

                if (item.Contains("ReceiptDocNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ReceiptDocNo) : query.ThenBy(x => x.ReceiptDocNo);
                }

                if (item.Contains("PaymentDocNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PaymentDocNo) : query.ThenBy(x => x.PaymentDocNo);
                }

                if (item.Contains("ReconcileDocNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ReconcileDocNo) : query.ThenBy(x => x.ReconcileDocNo);
                }

                if (item.Contains("InActive", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.InActive) : query.ThenBy(x => x.InActive);
                }

                if (item.Contains("IsCash", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.IsCash) : query.ThenBy(x => x.IsCash);
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
                query = query.ThenBy(x => x.CheckbookCode);
            }

            return query;
        }
    }
}
