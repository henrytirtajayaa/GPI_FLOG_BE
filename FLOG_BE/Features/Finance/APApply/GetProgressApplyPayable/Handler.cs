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
using Views = FLOG_BE.Model.Companies.View;
using FLOG_BE.Features.Finance.ARApply.GetHistoryApplyReceivable;

namespace FLOG_BE.Features.Finance.APApply.GetProgressApplyPayable
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

            var query = getTransaction(request.Initiator.UserId, request.Filter);
            query = getSorted(query, request.Sort);

            List<Person> ListUser = await GetUser();
            List<Vendor> ListVendor = await GetVendor();

            var list = await PaginatedList<Entities.APApplyHeader, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                PayableApplies = list,
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

        private RefApplyDoc GetApplyDocument(string documentType, Entities.APApplyHeader applyHeader, List<CheckbookTransactionHeader> queryAdvances,
          List<PayableTransactionHeader> queryCN,
          List<ApPaymentHeader> queryPayments)
        {
            RefApplyDoc reff = new RefApplyDoc();

            if (documentType.Equals(DOCUMENTTYPE.ADVANCE, StringComparison.OrdinalIgnoreCase))
            {
                var advancePayment = queryAdvances.Where(x => x.CheckbookTransactionId == applyHeader.CheckbookTransactionId).FirstOrDefault();
                if (advancePayment != null)
                {
                    reff.CurrencyCode = advancePayment.CurrencyCode;
                    reff.DocumentNo = advancePayment.DocumentNo;
                    reff.IsMultiply = advancePayment.IsMultiply;
                    reff.ExchangeRate = advancePayment.ExchangeRate;
                }
            }
            else if (documentType.Equals(DOCUMENTTYPE.PAYMENT, StringComparison.OrdinalIgnoreCase))
            {
                var apPayment = queryPayments.Where(x => x.PaymentHeaderId == applyHeader.PaymentHeaderId).FirstOrDefault();
                if (apPayment != null)
                {
                    reff.CurrencyCode = apPayment.CurrencyCode;
                    reff.DocumentNo = apPayment.DocumentNo;
                    reff.IsMultiply = apPayment.IsMultiply;
                    reff.ExchangeRate = apPayment.ExchangeRate;
                }
            }
            else if (documentType.Equals(DOCUMENTTYPE.CREDIT_NOTE, StringComparison.OrdinalIgnoreCase))
            {
                var cn = queryCN.Where(x => x.PayableTransactionId == applyHeader.PayableTransactionId).FirstOrDefault();
                if (cn != null)
                {
                    reff.CurrencyCode = cn.CurrencyCode;
                    reff.DocumentNo = cn.DocumentNo;
                    reff.IsMultiply = cn.IsMultiply;
                    reff.ExchangeRate = cn.ExchangeRate;
                }
            }

            return reff;
        }

        private IQueryable<Entities.APApplyHeader> getTransaction(string personId, RequestFilter filter)
        {
            List<Person> ListUser = _contextCentral.Persons.ToList();
            List<Vendor> litdata = _context.Vendors.ToList();

            List<PayableTransactionHeader> invoices = _context.PayableTransactionHeaders.ToList();

            var queryPayments = _context.ApPaymentHeaders.Where(x => x.Status == DOCSTATUS.POST).ToList();
            var queryAdvances = _context.CheckbookTransactionHeaders.Where(x => x.DocumentType.Equals(DOCUMENTTYPE.CHECKBOOK_OUT) && x.Status == DOCSTATUS.POST).ToList();
            var queryCN = _context.PayableTransactionHeaders.Where(x => x.DocumentType.Equals(DOCUMENTTYPE.CREDIT_NOTE) && x.Status == DOCSTATUS.POST).ToList();

            var availablesAdvances = _context.APPendingAdvancePayments.AsQueryable();
            var availablesCreditNotes = _context.APPendingCreditNotes.AsQueryable();
            var unapplyPayments = _context.APUnapplyPayments.AsQueryable();

            var query = (from x in _context.APApplyHeaders
                          .Where(x => (x.Status == DOCSTATUS.NEW || x.Status == DOCSTATUS.PROCESS))
                         select new Entities.APApplyHeader
                         {
                             PayableApplyId = x.PayableApplyId,
                             TransactionDate = x.TransactionDate,
                             PaymentHeaderId = x.PaymentHeaderId,
                             CheckbookTransactionId = x.CheckbookTransactionId,
                             PayableTransactionId = x.PayableTransactionId,
                             DocumentType = x.DocumentType,
                             DocumentNo = x.DocumentNo,
                             VendorId = x.VendorId,
                             VendorName = litdata.Where(p => p.VendorId == x.VendorId).Select(p => p.VendorName).FirstOrDefault(),
                             VendorCode = litdata.Where(p => p.VendorId == x.VendorId).Select(p => p.VendorCode).FirstOrDefault(),
                             Description = x.Description,
                             OriginatingTotalPaid = x.OriginatingTotalPaid,
                             FunctionalTotalPaid = x.FunctionalTotalPaid,
                             AvailableBalance = (
                             x.DocumentType.Equals(DOCUMENTTYPE.ADVANCE) ? (availablesAdvances.Where(c => c.CheckbookTransactionId == x.CheckbookTransactionId).Select(o => o.OriginatingBalance).FirstOrDefault()) :
                             (x.DocumentType.Equals(DOCUMENTTYPE.CREDIT_NOTE) ? (availablesCreditNotes.Where(c => c.PayableTransactionId == x.PayableTransactionId).Select(o => o.OriginatingBalance).FirstOrDefault()) :
                               (unapplyPayments.Where(c => c.PaymentHeaderId == x.PaymentHeaderId).Select(o => o.OriginatingBalance).FirstOrDefault()))
                             ) + x.OriginatingTotalPaid,
                             CurrencyCode = GetApplyDocument(x.DocumentType, x, queryAdvances, queryCN, queryPayments).CurrencyCode,
                             ExchangeRate = GetApplyDocument(x.DocumentType, x, queryAdvances, queryCN, queryPayments).ExchangeRate,
                             IsMultiply = GetApplyDocument(x.DocumentType, x, queryAdvances, queryCN, queryPayments).IsMultiply,
                             ReffDocumentNo = GetApplyDocument(x.DocumentType, x, queryAdvances, queryCN, queryPayments).DocumentNo,
                             CreatedBy = x.CreatedBy,
                             CreatedByName = ListUser.Where(p => p.PersonId == x.CreatedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             CreatedDate = x.CreatedDate,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedByName = ListUser.Where(p => p.PersonId == x.ModifiedBy).Select(p => p.PersonFullName).FirstOrDefault(),
                             Status = x.Status,
                             StatusComment = x.StatusComment,
                             APApplyDetails = (from s in _context.APApplyDetails
                                               where s.PayableApplyId == x.PayableApplyId
                                               select new Entities.APApplyDetail
                                               {
                                                   PayableApplyDetailId = s.PayableApplyDetailId,
                                                   PayableApplyId = s.PayableApplyId,
                                                   PayableTransactionId = s.PayableTransactionId,
                                                   DocumentNo = invoices.Where(z => z.PayableTransactionId == s.PayableTransactionId).Select(z => z.DocumentNo).FirstOrDefault(),
                                                   CurrencyCode = invoices.Where(z => z.PayableTransactionId == s.PayableTransactionId).Select(z => z.CurrencyCode).FirstOrDefault(),
                                                   VendorCode = x.VendorCode,
                                                   NsDocumentNo = invoices.Where(z => z.PayableTransactionId == s.PayableTransactionId).Select(z => z.NsDocumentNo).FirstOrDefault(),
                                                   OriginatingInvoice = invoices.Where(z => z.PayableTransactionId == s.PayableTransactionId).Select(z => (z.SubtotalAmount - z.DiscountAmount + z.TaxAmount)).FirstOrDefault(),
                                                   OrgDocAmount = invoices.Where(z => z.PayableTransactionId == s.PayableTransactionId).Select(z => (z.SubtotalAmount - z.DiscountAmount + z.TaxAmount)).FirstOrDefault(),
                                                   Description = s.Description,
                                                   OriginatingBalance = s.OriginatingBalance,
                                                   FunctionalBalance = s.FunctionalBalance,
                                                   OriginatingPaid = s.OriginatingPaid,
                                                   FunctionalPaid = s.FunctionalPaid,
                                                   Status = s.Status,
                                               }).ToList(),
                             
                         }).AsEnumerable().ToList().AsQueryable();

            var wherePredicates = PredicateBuilder.New<Entities.APApplyHeader>(true);
            
            var filterNo = filter.DocumentNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterNo)
                {
                    predicate = predicate.Or(x => x.DocumentNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterTransactionDateStart = filter.TransactionDateStart?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterTransactionDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterTransactionDateEnd = filter.TransactionDateEnd?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterTransactionDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterDocumentType = filter.DocumentType?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDocumentType.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterDocumentType)
                {
                    predicate = predicate.Or(x => x.DocumentType.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterReffDocumentNo = filter.ReffDocumentNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterReffDocumentNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterReffDocumentNo)
                {
                    predicate = predicate.Or(x => x.ReffDocumentNo.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterCurCode = filter.CurrencyCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCurCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterCurCode)
                {
                    predicate = predicate.Or(x => x.CurrencyCode.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterExcRateMin = filter.ExchangeRateMin?.Where(x => x > 0).ToList();
            if (filterExcRateMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterExcRateMin)
                {
                    predicate = predicate.Or(x => x.ExchangeRate >= filterItem);
                }
                wherePredicates.And(predicate);
            }
            var filterExcRateMax = filter.ExchangeRateMax?.Where(x => x > 0).ToList();
            if (filterExcRateMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterExcRateMax)
                {
                    predicate = predicate.Or(x => x.ExchangeRate <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterVendorName = filter.VendorName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterVendorName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterVendorName)
                {
                    predicate = predicate.Or(x => x.VendorName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterDescription = filter.Description?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDescription.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterDescription)
                {
                    predicate = predicate.Or(x => x.Description.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                wherePredicates.And(predicate);
            }

            var filterOriginatingTotalPaidMin = filter.OriginatingTotalPaidMin?.Where(x => x > 0).ToList();
            if (filterOriginatingTotalPaidMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterOriginatingTotalPaidMin)
                {
                    predicate = predicate.Or(x => x.OriginatingTotalPaid >= filterItem);
                }
                wherePredicates.And(predicate);
            }
            var filterOriginatingTotalPaidMax = filter.OriginatingTotalPaidMax?.Where(x => x > 0).ToList();
            if (filterOriginatingTotalPaidMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterOriginatingTotalPaidMax)
                {
                    predicate = predicate.Or(x => x.OriginatingTotalPaid <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            if (filter.Status.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                predicate = predicate.Or(x => x.Status == filter.Status);
                wherePredicates.And(predicate);
            }

            var filterStatusComment = filter.StatusComment?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterStatusComment.Any())
            {
                var predicate = PredicateBuilder.New<Entities.APApplyHeader>(false);
                foreach (var filterItem in filterStatusComment)
                {
                    predicate = predicate.Or(x => x.StatusComment.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);

            return query;
        }

        private IQueryable<Entities.APApplyHeader> getSorted(IQueryable<Entities.APApplyHeader> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.TransactionDate);

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

                if (item.Contains("DocumentType", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DocumentType) : query.ThenBy(x => x.DocumentType);
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
