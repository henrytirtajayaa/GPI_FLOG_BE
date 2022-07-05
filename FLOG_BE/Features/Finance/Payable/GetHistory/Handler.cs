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

namespace FLOG_BE.Features.Finance.Payable.GetHistory
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
            query = getPayableSorted(query, request.Sort);

            List<Person> ListUser = await GetUser();

            var list = await PaginatedList<Entities.PayableTransactionHeader, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());
            
            return ApiResult<Response>.Ok(new Response()
            {
                PayableTransaction = list,
                ListInfo = list.ListInfo
            });
        }
        private async Task<List<Person>> GetUser()
        {
            return await _contextCentral.Persons.ToListAsync();
        }
        private IQueryable<Entities.PayableTransactionHeader> getTransaction(string personId, RequestFilter filter)
        {

            List<Person> ListUser = _contextCentral.Persons.ToList();
            List<Vendor> litdata = _context.Vendors.ToList();
            List<Currency> listCurrency = _context.Currencies.ToList();
            List<NegotiationSheetHeader> listNS = _context.NegotiationSheetHeaders.ToList();

            var query = (from x in _context.PayableTransactionHeaders
                              .Where(x => x.Status != DOCSTATUS.NEW && x.Status != DOCSTATUS.PROCESS && x.Status != DOCSTATUS.APPROVE && x.Status != DOCSTATUS.DISAPPROVE && x.Status != DOCSTATUS.INACTIVE)
                         select new Entities.PayableTransactionHeader
                         {
                             PayableTransactionId = x.PayableTransactionId,
                             DocumentType = x.DocumentType,
                             DocumentNo = x.DocumentNo,
                             BranchCode = x.BranchCode ?? string.Empty,
                             TransactionDate = x.TransactionDate,
                             TransactionType = x.TransactionType,
                             CurrencyCode = x.CurrencyCode,
                             ExchangeRate = x.ExchangeRate,
                             VendorId = x.VendorId,
                             VendorName = litdata.Where(p => p.VendorId == x.VendorId).Select(p => p.VendorName).FirstOrDefault() ?? string.Empty,
                             VendorCode = litdata.Where(p => p.VendorId == x.VendorId).Select(p => p.VendorCode).FirstOrDefault() ?? string.Empty,
                             OriginatingExtendedAmount = (from s in _context.PayableTransactionDetails
                                                          where s.PayableTransactionId == x.PayableTransactionId
                                                          select s.OriginatingExtendedAmount).FirstOrDefault(),

                             PaymentTermCode = x.PaymentTermCode,
                             VendorAddressCode = x.VendorAddressCode,
                             VendorDocumentNo = x.VendorDocumentNo ?? string.Empty,
                             NsDocumentNo = x.NsDocumentNo,
                             MasterNo = listNS.Where(p => p.DocumentNo == x.NsDocumentNo).Select(p => p.MasterNo).FirstOrDefault() ?? string.Empty,
                             AgreementNo = listNS.Where(p => p.DocumentNo == x.NsDocumentNo).Select(p => p.AgreementNo).FirstOrDefault() ?? string.Empty,
                             Description = x.Description,
                             SubtotalAmount = x.SubtotalAmount,
                             DiscountAmount = x.DiscountAmount,
                             TaxAmount = x.TaxAmount,
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
                             BillToAddressCode = x.BillToAddressCode,
                             ShipToAddressCode = x.ShipToAddressCode,
                             DecimalPlaces = listCurrency.Where(c => c.CurrencyCode.Equals(x.CurrencyCode, StringComparison.OrdinalIgnoreCase)).Select(s => s.DecimalPlaces).FirstOrDefault(),
                             PayableTransactionDetails = (from s in _context.PayableTransactionDetails
                                                          where s.PayableTransactionId == x.PayableTransactionId
                                                          select new Entities.PayableTransactionDetail
                                                          {
                                                              PayableTransactionId = s.PayableTransactionId,
                                                              ChargesId = s.ChargesId,
                                                              ChargesCode = (from z in _context.Charges
                                                                             where z.ChargesId == s.ChargesId
                                                                             select z.ChargesCode).FirstOrDefault(),
                                                              ChargesName = (from z in _context.Charges
                                                                             where z.ChargesId == s.ChargesId
                                                                             select z.ChargesName).FirstOrDefault(),
                                                              ChargesDescription = s.ChargesDescription,
                                                              OriginatingAmount = s.OriginatingAmount,
                                                              OriginatingTax = s.OriginatingTax,
                                                              OriginatingDiscount = s.OriginatingDiscount,
                                                              OriginatingExtendedAmount = s.OriginatingExtendedAmount,
                                                              FunctionalTax = s.FunctionalTax,
                                                              FunctionalDiscount = s.FunctionalDiscount,
                                                              FunctionalExtendedAmount = s.FunctionalExtendedAmount,
                                                              Status = s.Status,
                                                              TaxScheduleId = s.TaxScheduleId,
                                                              IsTaxAfterDiscount = s.IsTaxAfterDiscount,
                                                              TaxScheduleCode = (s.TaxScheduleId != Guid.Empty ? _context.TaxSchedules.Where(z => z.TaxScheduleId == s.TaxScheduleId).Select(z => z.TaxScheduleCode).FirstOrDefault() : ""),
                                                              TaxablePercentTax = (s.TaxScheduleId != Guid.Empty ? _context.TaxSchedules.Where(z => z.TaxScheduleId == s.TaxScheduleId).Select(z => z.TaxablePercent).FirstOrDefault() : 0),
                                                              PercentDiscount = s.PercentDiscount
                                                          }).ToList(),

                             PayableTransactionTaxes = (from s in _context.PayableTransactionTaxes
                                                        where s.PayableTransactionId == x.PayableTransactionId
                                                        select s).ToList(),

                         }).AsEnumerable().ToList().AsQueryable();

            var wherePredicates = PredicateBuilder.New<Entities.PayableTransactionHeader>(true);
            var filterDocType = filter.DocumentType?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDocType.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterDocType)
                {
                    predicate = predicate.Or(x => x.DocumentType.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterNo = filter.DocumentNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterNo)
                {
                    predicate = predicate.Or(x => x.DocumentNo.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterTransactionDateStart = filter.TransactionDateStart?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterTransactionDateStart)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterTransactionDateEnd = filter.TransactionDateEnd?.Where(x => x.HasValue).ToList();
            if (filterTransactionDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterTransactionDateEnd)
                {
                    predicate = predicate.Or(x => ((DateTime)x.TransactionDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }


            var filterTransType = filter.TransactionType?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTransType.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterTransType)
                {
                    predicate = predicate.Or(x => x.TransactionType.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCurCode = filter.CurrencyCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCurCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterCurCode)
                {
                    predicate = predicate.Or(x => x.CurrencyCode.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterExcRateMin = filter.ExchangeRateMin?.Where(x => x > 0).ToList();
            if (filterExcRateMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterExcRateMin)
                {
                    predicate = predicate.Or(x => x.ExchangeRate >= filterItem);
                }
                wherePredicates.And(predicate);
            }
            var filterExcRateMax = filter.ExchangeRateMax?.Where(x => x > 0).ToList();
            if (filterExcRateMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterExcRateMax)
                {
                    predicate = predicate.Or(x => x.ExchangeRate <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterPaymentTermCode = filter.PaymentTermCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterPaymentTermCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterTransType)
                {
                    predicate = predicate.Or(x => x.PaymentTermCode.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            if (filter.VendorId != null && filter.VendorId != Guid.Empty)
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                predicate = predicate.Or(x => x.VendorId == filter.VendorId);

                wherePredicates.And(predicate);
            }
            var filterVendorName = filter.VendorName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterVendorName.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterVendorName)
                {
                    predicate = predicate.Or(x => x.VendorName.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterVendorAddressCode = filter.VendorAddressCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterVendorAddressCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterVendorAddressCode)
                {
                    predicate = predicate.Or(x => x.VendorAddressCode.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterSoDocumentNo = filter.VendorDocumentNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterSoDocumentNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterSoDocumentNo)
                {
                    predicate = predicate.Or(x => x.VendorDocumentNo.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterNsDocumentNo = filter.NsDocumentNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterNsDocumentNo.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterNsDocumentNo)
                {
                    predicate = predicate.Or(x => x.NsDocumentNo.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterDescription = filter.Description?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterDescription.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterDescription)
                {
                    predicate = predicate.Or(x => x.Description.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }


            var filterSubtotalAmountMin = filter.SubtotalAmountMin?.Where(x => x > 0).ToList();
            if (filterSubtotalAmountMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterSubtotalAmountMin)
                {
                    predicate = predicate.Or(x => x.SubtotalAmount >= filterItem);
                }
                wherePredicates.And(predicate);
            }
            var filterSubtotalAmountMax = filter.SubtotalAmountMax?.Where(x => x > 0).ToList();
            if (filterSubtotalAmountMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterSubtotalAmountMax)
                {
                    predicate = predicate.Or(x => x.SubtotalAmount <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterDiscountAmountMin = filter.DiscountAmountMin?.Where(x => x > 0).ToList();
            if (filterDiscountAmountMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterDiscountAmountMin)
                {
                    predicate = predicate.Or(x => x.DiscountAmount >= filterItem);
                }
                wherePredicates.And(predicate);
            }
            var filterDiscountAmountMax = filter.DiscountAmountMax?.Where(x => x > 0).ToList();
            if (filterDiscountAmountMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterDiscountAmountMax)
                {
                    predicate = predicate.Or(x => x.DiscountAmount <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterTaxAmountMin = filter.TaxAmountMin?.Where(x => x > 0).ToList();
            if (filterTaxAmountMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterTaxAmountMin)
                {
                    predicate = predicate.Or(x => x.TaxAmount >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterTaxAmountMax = filter.TaxAmountMax?.Where(x => x > 0).ToList();
            if (filterTaxAmountMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterTaxAmountMax)
                {
                    predicate = predicate.Or(x => x.TaxAmount <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterVoidBy = filter.VoidBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterVoidBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterVoidBy)
                {
                    predicate = predicate.Or(x => x.VoidBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterVoidStart = filter.VoidDateStart?.Where(x => x.HasValue).ToList();
            if (filterVoidStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterVoidStart)
                {
                    predicate = predicate.Or(x => x.VoidDate.HasValue && ((DateTime)x.VoidDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterVoidEnd = filter.VoidDateEnd?.Where(x => x.HasValue).ToList();
            if (filterVoidEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterVoidEnd)
                {
                    predicate = predicate.Or(x => x.VoidDate.HasValue && ((DateTime)x.VoidDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedBy = filter.CreatedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterCreatedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterCreatedBy)
                {
                    predicate = predicate.Or(x => x.CreatedBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateStart = filter.CreatedDateStart?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterCreatedDateStart)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterCreatedDateEnd = filter.CreatedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterCreatedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterCreatedDateEnd)
                {
                    predicate = predicate.Or(x => x.CreatedDate.HasValue && ((DateTime)x.CreatedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedBy = filter.ModifiedBy?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterModifiedBy.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterModifiedBy)
                {
                    predicate = predicate.Or(x => x.ModifiedBy.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateStart = filter.ModifiedDateStart?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateStart.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterModifiedDateStart)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date >= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterModifiedDateEnd = filter.ModifiedDateEnd?.Where(x => x.HasValue).ToList();
            if (filterModifiedDateEnd.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterModifiedDateEnd)
                {
                    predicate = predicate.Or(x => x.ModifiedDate.HasValue && ((DateTime)x.ModifiedDate).Date <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            if (filter.Status.HasValue)
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                predicate = predicate.Or(x => x.Status == filter.Status);
                wherePredicates.And(predicate);
            }

            var filterStatusComment = filter.StatusComment?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterStatusComment.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterStatusComment)
                {
                    predicate = predicate.Or(x => x.StatusComment.Contains(filterItem));
                }
                wherePredicates.And(predicate);
            }

            var filterAddressCode = filter.BillToAddressCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterAddressCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterAddressCode)
                {
                    predicate = predicate.Or(x => x.BillToAddressCode.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }
            var filterShipAddressCode = filter.ShipToAddressCode?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterShipAddressCode.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterShipAddressCode)
                {
                    predicate = predicate.Or(x => x.ShipToAddressCode.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterOriginatingExtendedAmountMin = filter.OriginatingExtendedAmountMin?.Where(x => x > 0).ToList();
            if (filterOriginatingExtendedAmountMin.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterOriginatingExtendedAmountMin)
                {
                    predicate = predicate.Or(x => x.OriginatingExtendedAmount >= filterItem);
                }
                wherePredicates.And(predicate);
            }
            var filterOriginatingExtendedAmountMax = filter.OriginatingExtendedAmountMax?.Where(x => x > 0).ToList();
            if (filterOriginatingExtendedAmountMax.Any())
            {
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterOriginatingExtendedAmountMax)
                {
                    predicate = predicate.Or(x => x.OriginatingExtendedAmount <= filterItem);
                }
                wherePredicates.And(predicate);
            }

            var filterMasterNo = filter.MasterNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterMasterNo.Any())
            {
                
                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterMasterNo)
                {
                    predicate = predicate.Or(x => x.MasterNo.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            var filterAgreementNo = filter.AgreementNo?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterAgreementNo.Any())
            {

                var predicate = PredicateBuilder.New<Entities.PayableTransactionHeader>(false);
                foreach (var filterItem in filterAgreementNo)
                {
                    predicate = predicate.Or(x => x.AgreementNo.ToLower().Contains(filterItem.ToLower()));
                }
                wherePredicates.And(predicate);
            }

            query = query.Where(wherePredicates);

            return query;
            }

        private IQueryable<Entities.PayableTransactionHeader> getPayableSorted(IQueryable<Entities.PayableTransactionHeader> input, List<string> sort)
        {
            var query = input.OrderBy(x => 0);

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
                if (item.Contains("PaymentTermCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.PaymentTermCode) : query.ThenBy(x => x.PaymentTermCode);
                }

                if (item.Contains("VendorAddressCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VendorAddressCode) : query.ThenBy(x => x.VendorAddressCode);
                }

                if (item.Contains("VendorDocumentNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.VendorDocumentNo) : query.ThenBy(x => x.VendorDocumentNo);
                }

                if (item.Contains("NsDocumentNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.NsDocumentNo) : query.ThenBy(x => x.NsDocumentNo);
                }
                if (item.Contains("MasterNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.MasterNo) : query.ThenBy(x => x.MasterNo);
                }
                if (item.Contains("AgreementNo", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.AgreementNo) : query.ThenBy(x => x.AgreementNo);
                }
                if (item.Contains("Description", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.Description) : query.ThenBy(x => x.Description);
                }
                if (item.Contains("SubtotalAmount", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.SubtotalAmount) : query.ThenBy(x => x.SubtotalAmount);
                }
                if (item.Contains("DiscountAmount", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.DiscountAmount) : query.ThenBy(x => x.DiscountAmount);
                }

                if (item.Contains("TaxAmount", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.TaxAmount) : query.ThenBy(x => x.TaxAmount);
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

                if (item.Contains("StatusComment", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.StatusComment) : query.ThenBy(x => x.StatusComment);
                }

                if (item.Contains("VendorBillAddressCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.BillToAddressCode) : query.ThenBy(x => x.BillToAddressCode);
                }

                if (item.Contains("VendorShipAddressCode", StringComparison.InvariantCultureIgnoreCase))
                {
                    query = item.StartsWith('-') ? query.ThenByDescending(x => x.ShipToAddressCode) : query.ThenBy(x => x.ShipToAddressCode);
                }

                //if (item.Contains("OriginatingExtendedAmount", StringComparison.InvariantCultureIgnoreCase))
                //{
                //    query = item.StartsWith('-') ? query.ThenByDescending(x => x.OriginatingExtendedAmount) : query.ThenBy(x => x.OriginatingExtendedAmount);
                //}
            }

            if (!sortingList.Any())
            {
                query = query.ThenBy(x => x.DocumentNo).ThenBy(x => x.TransactionDate);
            }

            return query;
        }
    }
}
