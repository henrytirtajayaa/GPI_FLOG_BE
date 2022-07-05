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

namespace FLOG_BE.Features.Finance.DepositSettlement.GetHistory
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

            var query = GetSettlement(request.Initiator.UserId, request.Filter);
            query = GetSettlementSorted(query, request.Sort);

            List<Person> ListUser = await GetUser();
            List<Customer> ListCustomer = await GetCustomer();

            var list = await PaginatedList<Entities.DepositSettlementHeader, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());


            return ApiResult<Response>.Ok(new Response()
            {
                DepositSettlement = list,
                ListInfo = list.ListInfo
            });
        }

        private async Task<List<Person>> GetUser()
        {
            return await _contextCentral.Persons.ToListAsync();
        }
        private async Task<List<Customer>> GetCustomer()
        {
            return await _context.Customers.ToListAsync();
        }

        private IQueryable<Entities.DepositSettlementHeader> GetSettlement(string personId, RequestFilter filter)
        {
            List<Person> ListUser = _contextCentral.Persons.ToList();
            List<Customer> litdata = _context.Customers.ToList();
            List<ReceivableTransactionHeader> litReceivable = _context.ReceivableTransactionHeaders.ToList();


            var query = (from x in _context.DepositSettlementHeaders
                         .Where(x => x.Status != DOCSTATUS.NEW && x.Status != DOCSTATUS.PROCESS && x.Status != DOCSTATUS.APPROVE && x.Status != DOCSTATUS.DISAPPROVE && x.Status != DOCSTATUS.INACTIVE || (x.Status == DOCSTATUS.POST))
                         select new Entities.DepositSettlementHeader
                         {
                             SettlementHeaderId = x.SettlementHeaderId,
                             TransactionDate = x.TransactionDate,
                             DocumentType = x.DocumentType,
                             DocumentNo = x.DocumentNo,
                             ReceiveTransactionId = x.ReceiveTransactionId,
                             DepositNo = x.DepositNo,
                             ApplyDocumentNo = x.ApplyDocumentNo,
                             CheckbookDocumentNo = x.CheckbookDocumentNo,
                             CurrencyCode = x.CurrencyCode,
                             ExchangeRate = x.ExchangeRate,
                             CheckbookCode = x.CheckbookCode,
                             CustomerId = x.CustomerId,
                             CustomerCode = litdata.Where(p => p.CustomerId == x.CustomerId).Select(p => p.CustomerCode).FirstOrDefault(),
                             CustomerName = litdata.Where(p => p.CustomerId == x.CustomerId).Select(p => p.CustomerName).FirstOrDefault(),
                             Description = x.Description,
                             AppliedTotalPaid = (from v in _context.DepositSettlementDetails
                                                 join a in _context.DepositSettlementHeaders on v.SettlementHeaderId equals a.SettlementHeaderId
                                                 where a.SettlementHeaderId == x.SettlementHeaderId
                                                 select v).Sum(v => v.OriginatingPaid),
                             OriginatingTotalPaid = x.OriginatingTotalPaid,
                             FunctionalTotalPaid = x.FunctionalTotalPaid,
                             CreatedBy = x.CreatedBy,
                             CreatedByName = ListUser.Where(p => p.PersonId == x.CreatedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             CreatedDate = x.CreatedDate,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedByName = ListUser.Where(p => p.PersonId == x.ModifiedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             VoidBy = x.VoidBy,
                             VoidByName = ListUser.Where(p => p.PersonId == x.VoidBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             VoidDate = x.VoidDate,
                             Status = x.Status,
                             StatusComment = x.StatusComment,
                             DepositSettlementDetails = (from s in _context.DepositSettlementDetails
                                                         where s.SettlementHeaderId == x.SettlementHeaderId && (s.Status == DOCSTATUS.NEW || s.Status == DOCSTATUS.POST)
                                                         select new Entities.DepositSettlementDetail
                                                         {
                                                             SettlementDetailId = s.SettlementDetailId,
                                                             SettlementHeaderId = s.SettlementHeaderId,
                                                             ReceiveTransactionId = s.ReceiveTransactionId,
                                                             DocumentNo = litReceivable.Where(z => z.ReceiveTransactionId == s.ReceiveTransactionId).Select(z => z.DocumentNo).FirstOrDefault(),
                                                             CustomerName = x.CustomerName,
                                                             NsDocumentNo = litReceivable.Where(z => z.ReceiveTransactionId == s.ReceiveTransactionId).Select(z => z.NsDocumentNo).FirstOrDefault(),
                                                             OrgDocAmount = litReceivable.Where(z => z.ReceiveTransactionId == s.ReceiveTransactionId).Select(z => z.SubtotalAmount - z.DiscountAmount + z.TaxAmount).FirstOrDefault(),
                                                             Description = s.Description,
                                                             OriginatingBalance = s.OriginatingBalance,
                                                             FunctionalBalance = s.FunctionalBalance,
                                                             OriginatingPaid = s.OriginatingPaid,
                                                             FunctionalPaid = s.FunctionalPaid,
                                                             Status = s.Status,
                                                         }).ToList()
                         }).AsEnumerable().ToList().AsQueryable();

            var wherePredicates = PredicateBuilder.New<Entities.DepositSettlementHeader>(true);
            var filterTransactionDateStart = filter.TransactionDateStart?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterTransactionDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterTransactionDateEnd = filter.TransactionDateEnd?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterTransactionDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterTransactionType = filter.DocumentType?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTransactionType.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterTransactionType)
                {
                    predicate = predicate.Or(x => x.DocumentType.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterDocumentNo = filter.DocumentNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDocumentNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterDocumentNo)
                {
                    predicate = predicate.Or(x => x.DocumentNo.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterDepositNo = filter.DepositNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDepositNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterDepositNo)
                {
                    predicate = predicate.Or(x => x.DepositNo.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCurrencyCode = filter.CurrencyCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCurrencyCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterCurrencyCode)
                {
                    predicate = predicate.Or(x => x.CurrencyCode.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterExcRateMin = filter.ExchangeRateMin?.Where(x => x > 0).ToList();
            if (filterExcRateMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterExcRateMin)
                {
                    predicate = predicate.Or(x => x.ExchangeRate >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterExcRateMax = filter.ExchangeRateMax?.Where(x => x > 0).ToList();
            if (filterExcRateMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterExcRateMax)
                {
                    predicate = predicate.Or(x => x.ExchangeRate <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCheckbookCode = filter.CheckbookCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCheckbookCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterCheckbookCode)
                {
                    predicate = predicate.Or(x => x.CheckbookCode.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCustomerName = filter.CustomerName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCustomerName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterCustomerName)
                {
                    predicate = predicate.Or(x => x.CustomerName.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterDescription = filter.Description?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDescription.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterDescription)
                {
                    predicate = predicate.Or(x => x.Description.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterOriginatingTotalPaidMin = filter.OriginatingTotalPaidMin?.Where(x => x > 0).ToList();
            if (filterOriginatingTotalPaidMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterOriginatingTotalPaidMin)
                {
                    predicate = predicate.Or(x => x.OriginatingTotalPaid >= filterItem);
                }
                wherePredicates.And(predicate);
            }
            var filterOriginatingTotalPaidMax = filter.OriginatingTotalPaidMax?.Where(x => x > 0).ToList();
            if (filterOriginatingTotalPaidMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterOriginatingTotalPaidMax)
                {
                    predicate = predicate.Or(x => x.OriginatingTotalPaid <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterFunctionalTotalPaidMin = filter.FunctionalTotalPaidMin?.Where(x => x > 0).ToList();
            if (filterFunctionalTotalPaidMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterFunctionalTotalPaidMin)
                {
                    predicate = predicate.Or(x => x.FunctionalTotalPaid >= filterItem);
                }
                wherePredicates.And(predicate);
            }
            var filterFunctionalTotalPaidMax = filter.FunctionalTotalPaidMax?.Where(x => x > 0).ToList();
            if (filterFunctionalTotalPaidMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterFunctionalTotalPaidMax)
                {
                    predicate = predicate.Or(x => x.FunctionalTotalPaid <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterVoidBy = filter.VoidBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterVoidBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterVoidBy)
                {
                    predicate = predicate.Or(x => x.VoidBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterVoidStart = filter.VoidDateStart?.Where(x => x.HasValue).ToList();
            if (filterVoidStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterVoidStart)
                {
                    predicate = predicate.Or(x => x.VoidDate.HasValue && ((DateTime)x.VoidDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterVoidEnd = filter.VoidDateEnd?.Where(x => x.HasValue).ToList();
            if (filterVoidEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterVoidEnd)
                {
                    predicate = predicate.Or(x => x.VoidDate.HasValue && ((DateTime)x.VoidDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            if (filter.Status.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                predicate = predicate.Or(x => x.Status == filter.Status);
                wherePredicates.And(predicate);
            }

            var filterStatusComment = filter.StatusComment?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterStatusComment.Any())
            {
                var predicate = PredicateBuilder.New<Entities.DepositSettlementHeader>(false);
                foreach (var filterItem in filterStatusComment)
                {
                    predicate = predicate.Or(x => x.StatusComment.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);

            return query;
        }

        private IQueryable<Entities.DepositSettlementHeader> GetSettlementSorted(IQueryable<Entities.DepositSettlementHeader> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
                if (item.Contains("TransactionDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TransactionDate) : query.ThenBy(x => x.TransactionDate);
                }

                if (item.Contains("DocumentType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DocumentType) : query.ThenBy(x => x.DocumentType);
                }

                if (item.Contains("DocumentNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DocumentNo) : query.ThenBy(x => x.DocumentNo);
                }

                if (item.Contains("DepositNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DepositNo) : query.ThenBy(x => x.DepositNo);
                }

                if (item.Contains("CurrencyCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CurrencyCode) : query.ThenBy(x => x.CurrencyCode);
                }

                if (item.Contains("ExchangeRate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ExchangeRate) : query.ThenBy(x => x.ExchangeRate);
                }

                if (item.Contains("CheckbookCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CheckbookCode) : query.ThenBy(x => x.CheckbookCode);
                }

                if (item.Contains("CustomerName", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CustomerName) : query.ThenBy(x => x.CustomerName);
                }

                if (item.Contains("Description", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Description) : query.ThenBy(x => x.Description);
                }

                if (item.Contains("OriginatingTotalPaid", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.OriginatingTotalPaid) : query.ThenBy(x => x.OriginatingTotalPaid);
                }

                if (item.Contains("FunctionalTotalPaid", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.FunctionalTotalPaid) : query.ThenBy(x => x.FunctionalTotalPaid);
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

                if (item.Contains("VoidBy", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VoidBy) : query.ThenBy(x => x.VoidBy);
                }

                if (item.Contains("VoidDate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VoidDate) : query.ThenBy(x => x.VoidDate);
                }

                if (item.Contains("Status", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Status) : query.ThenBy(x => x.Status);
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
