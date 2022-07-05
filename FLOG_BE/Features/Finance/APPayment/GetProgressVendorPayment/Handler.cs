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

namespace FLOG_BE.Features.Finance.ApPayment.GetProgressVendorPayment
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

            var query = getPaymentProgress(request.Initiator.UserId, request.Filter);
            query = getPaymentSorted(query, request.Sort);

            List<Person> ListUser = await GetUser();
            List<Vendor> ListVendor = await GetVendor();

            var list = await PaginatedList<Entities.ApPaymentHeader, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                PaymentHeader = list,
                ListInfo = list.ListInfo
            });
        }
        private async Task<List<Person>> GetUser()
        {
            return await _contextCentral.Persons.ToListAsync();
        }
        private async Task<List<Vendor>> GetVendor()
        {
            return await _context.Vendors.ToListAsync();
        }

     
        private IQueryable<Entities.ApPaymentHeader> getPaymentProgress(string personId, RequestFilter filter)
        {
            List<Person> ListUser = _contextCentral.Persons.ToList();
            List<Vendor> litdata = _context.Vendors.ToList();
            List<PayableTransactionHeader> litPayable = _context.PayableTransactionHeaders.ToList();
            List<SalesOrderHeader> ListSO = _context.SalesOrderHeaders.ToList();

            var query = (from x in _context.ApPaymentHeaders
                          .Where(x => x.Status == DOCSTATUS.NEW || x.Status == DOCSTATUS.PROCESS || x.Status == DOCSTATUS.APPROVE || x.Status == DOCSTATUS.DISAPPROVE)
                         select new Entities.ApPaymentHeader
                         {
                             PaymentHeaderId = x.PaymentHeaderId,
                             TransactionDate = x.TransactionDate,
                             TransactionType = x.TransactionType,
                             DocumentNo = x.DocumentNo,
                             CurrencyCode = x.CurrencyCode,
                             ExchangeRate = x.ExchangeRate,
                             IsMultiply = x.IsMultiply,
                             CheckbookCode = x.CheckbookCode,
                             VendorId = x.VendorId,
                             VendorCode = litdata.Where(p => p.VendorId == x.VendorId).Select(p => p.VendorCode).FirstOrDefault(),
                             VendorName = litdata.Where(p => p.VendorId == x.VendorId).Select(p => p.VendorName).FirstOrDefault(),
                             Description = x.Description,
                             OriginatingTotalInvoice = (from iv in _context.PayableTransactionHeaders
                                                        join det in _context.ApPaymentDetails on iv.PayableTransactionId equals det.PayableTransactionId
                                                        where det.PaymentHeaderId == x.PaymentHeaderId
                                                        select iv ).Sum(iv=>iv.SubtotalAmount-iv.DiscountAmount+iv.TaxAmount),
                             AppliedTotalPaid = ( from v in _context.ApPaymentDetails
                                                  join a in _context.ApPaymentHeaders on v.PaymentHeaderId equals a.PaymentHeaderId
                                                  where a.PaymentHeaderId == x.PaymentHeaderId
                                                  select v ).Sum(v=>v.OriginatingPaid),
                             OriginatingTotalPaid = x.OriginatingTotalPaid,
                             FunctionalTotalPaid = x.FunctionalTotalPaid,
                             CreatedBy = x.CreatedBy,
                             CreatedByName = ListUser.Where(p => p.PersonId == x.CreatedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             CreatedDate = x.CreatedDate,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedByName = ListUser.Where(p => p.PersonId == x.ModifiedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             Status = x.Status,
                             StatusComment = x.StatusComment,
                             ApPaymentDetails = (from s in _context.ApPaymentDetails
                                                 where s.PaymentHeaderId == x.PaymentHeaderId
                                                 join b in _context.PayableTransactionHeaders
                                                 on s.PayableTransactionId equals b.PayableTransactionId into JoinA from ja in JoinA.DefaultIfEmpty()
                                                 join c in _context.NegotiationSheetHeaders
                                                 on ja.NsDocumentNo equals c.DocumentNo into JoinB from jb in JoinB.DefaultIfEmpty()
                                                 join d in _context.SalesOrderHeaders
                                                 on jb.SalesOrderId equals d.SalesOrderId into JoinTable
                                                 from jt in JoinTable.DefaultIfEmpty()
                                                 select new Entities.ApPaymentDetail
                                                 {
                                                     PaymentDetailId = s.PaymentDetailId,
                                                     PaymentHeaderId = s.PaymentHeaderId,
                                                     PayableTransactionId = s.PayableTransactionId,
                                                     DocumentNo = litPayable.Where(z => z.PayableTransactionId == s.PayableTransactionId).Select(z => z.DocumentNo).FirstOrDefault(),
                                                     VendorCode = x.VendorCode,
                                                     NsDocumentNo = litPayable.Where(z => z.PayableTransactionId == s.PayableTransactionId).Select(z => z.NsDocumentNo).FirstOrDefault(),
                                                     MasterNo = ListSO.Where(p => p.SalesOrderId == jt.SalesOrderId).Select(p => p.MasterNo).FirstOrDefault(),
                                                     AgreementNo = ListSO.Where(p => p.SalesOrderId == jt.SalesOrderId).Select(p => p.AgreementNo).FirstOrDefault(),
                                                     OrgDocAmount = litPayable.Where(z => z.PayableTransactionId == s.PayableTransactionId).Select(z => z.SubtotalAmount-z.DiscountAmount+z.TaxAmount).FirstOrDefault(),
                                                     Description = s.Description,
                                                     OriginatingBalance = s.OriginatingBalance,
                                                     FunctionalBalance = s.FunctionalBalance,
                                                     OriginatingPaid = s.OriginatingPaid,
                                                     FunctionalPaid = s.FunctionalPaid,
                                                     Status = s.Status,
                                                 }).ToList(),

                         }).AsEnumerable().ToList().AsQueryable();

            var wherePredicates = PredicateBuilder.New<Entities.ApPaymentHeader>(true);
          
            var filterNo = filter.DocumentNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterNo)
                {
                    predicate = predicate.Or(x => x.DocumentNo.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterTransactionDateStart = filter.TransactionDateStart?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterTransactionDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterTransactionDateEnd = filter.TransactionDateEnd?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterTransactionDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }


            var filterTransType = filter.TransactionType?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTransType.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterTransType)
                {
                    predicate = predicate.Or(x => x.TransactionType.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCheckbookCode = filter.CheckbookCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCheckbookCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterCheckbookCode)
                {
                    predicate = predicate.Or(x => x.CheckbookCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterCurCode = filter.CurrencyCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCurCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterCurCode)
                {
                    predicate = predicate.Or(x => x.CurrencyCode.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterExcRateMin = filter.ExchangeRateMin?.Where(x => x > 0).ToList();
            if (filterExcRateMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterExcRateMin)
                {
                    predicate = predicate.Or(x => x.ExchangeRate >= filterItem);
                }
                wherePredicates.And(predicate);
            }
            var filterExcRateMax = filter.ExchangeRateMax?.Where(x => x > 0).ToList();
            if (filterExcRateMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterExcRateMax)
                {
                    predicate = predicate.Or(x => x.ExchangeRate <= filterItem);
                }
                wherePredicates.And(predicate);
            }

           
            var filterVendorName = filter.VendorName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterVendorName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterVendorName)
                {
                    predicate = predicate.Or(x => x.VendorName.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

           
            var filterDescription = filter.Description?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDescription.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterDescription)
                {
                    predicate = predicate.Or(x => x.Description.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }


            var filterOriginatingTotalPaidMin = filter.OriginatingTotalPaidMin?.Where(x => x > 0).ToList();
            if (filterOriginatingTotalPaidMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterOriginatingTotalPaidMin)
                {
                    predicate = predicate.Or(x => x.OriginatingTotalPaid >= filterItem);
                }
                wherePredicates.And(predicate);
            }
            var filterOriginatingTotalPaidMax = filter.OriginatingTotalPaidMax?.Where(x => x > 0).ToList();
            if (filterOriginatingTotalPaidMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterOriginatingTotalPaidMax)
                {
                    predicate = predicate.Or(x => x.OriginatingTotalPaid <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            if (filter.Status.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                predicate = predicate.Or(x => x.Status == filter.Status);
                wherePredicates.And(predicate);
            }

            var filterStatusComment = filter.StatusComment?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterStatusComment.Any())
            {
                var predicate = PredicateBuilder.New<Entities.ApPaymentHeader>(false);
                foreach (var filterItem in filterStatusComment)
                {
                    predicate = predicate.Or(x => x.StatusComment.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);

            return query;
        }

        private IQueryable<Entities.ApPaymentHeader> getPaymentSorted(IQueryable<Entities.ApPaymentHeader> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

            var sortingList = sort.Where(x => !string.IsNullOrEmpty(x)).Select(x => x.TrimStart());
            foreach (var item in sortingList)
            {
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

                if (item.Contains("CheckbookCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CheckbookCode) : query.ThenBy(x => x.CheckbookCode);
                }

                if (item.Contains("CurrencyCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.CurrencyCode) : query.ThenBy(x => x.CurrencyCode);
                }

                if (item.Contains("ExchangeRate", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ExchangeRate) : query.ThenBy(x => x.ExchangeRate);
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
