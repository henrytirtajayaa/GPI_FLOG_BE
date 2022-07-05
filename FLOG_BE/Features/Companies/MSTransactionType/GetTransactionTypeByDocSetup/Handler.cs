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
using FLOG.Core;
using FLOG.Core.DocumentNo;

namespace FLOG_BE.Features.Companies.MSTransactionType.GetTransactionTypeByDocSetup
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

            var list = await PaginatedList<ResponseItem, ResponseItem>.CreateAsync(_linkCollection, query, request.Offset, request.Limit, query.ToList().Count());

            return ApiResult<Response>.Ok(new Response()
            {
                DocumentTypes = GetDocumentTypes(request.Filter),
                TransactionTypes = list,
                ListInfo = list.ListInfo
            });
        }

        private List<ResponseDocType> GetDocumentTypes(RequestFilter filter)
        {
            List<ResponseDocType> doctypes = new List<ResponseDocType>();

            if (filter.TrxModule == TRX_MODULE.TRX_RECEIVABLE)
            {
                doctypes.Add(new ResponseDocType { DocFeatureId = DOCNO_FEATURE.TRXTYPE_INVOICE, Text = DOCUMENTTYPE.INVOICE, Value = DOCUMENTTYPE.INVOICE });
                doctypes.Add(new ResponseDocType { DocFeatureId = DOCNO_FEATURE.TRXTYPE_DEBITNOTE, Text = DOCUMENTTYPE.DEBIT_NOTE, Value = DOCUMENTTYPE.DEBIT_NOTE });
                doctypes.Add(new ResponseDocType { DocFeatureId = DOCNO_FEATURE.TRXTYPE_CREDITNOTE, Text = DOCUMENTTYPE.CREDIT_NOTE, Value = DOCUMENTTYPE.CREDIT_NOTE });
            }
            else if (filter.TrxModule == TRX_MODULE.TRX_PAYABLE)
            {
                doctypes.Add(new ResponseDocType { DocFeatureId = DOCNO_FEATURE.TRXTYPE_INVOICE, Text = DOCUMENTTYPE.INVOICE, Value = DOCUMENTTYPE.INVOICE });
                doctypes.Add(new ResponseDocType { DocFeatureId = DOCNO_FEATURE.TRXTYPE_DEBITNOTE, Text = DOCUMENTTYPE.DEBIT_NOTE, Value = DOCUMENTTYPE.DEBIT_NOTE });
                doctypes.Add(new ResponseDocType { DocFeatureId = DOCNO_FEATURE.TRXTYPE_CREDITNOTE, Text = DOCUMENTTYPE.CREDIT_NOTE, Value = DOCUMENTTYPE.CREDIT_NOTE });
            }
            else if (filter.TrxModule == TRX_MODULE.TRX_SALES)
            {
                doctypes.Add(new ResponseDocType { DocFeatureId = DOCNO_FEATURE.TRXTYPE_SALES_QUOT, Text = "QUOTATION", Value = "QUOTATION" });
                doctypes.Add(new ResponseDocType { DocFeatureId = DOCNO_FEATURE.TRXTYPE_SALES_ORDER, Text = "SALES ORDER", Value = "SALES ORDER" });
                doctypes.Add(new ResponseDocType { DocFeatureId = DOCNO_FEATURE.TRXTYPE_SALES_NEGOSHEET, Text = "NEGOTIATION SHEET", Value = "NEGOTIATION SHEET" });
            }
            else if (filter.TrxModule == TRX_MODULE.TRX_CONTAINER_RENT)
            {
                doctypes.Add(new ResponseDocType { DocFeatureId = DOCNO_FEATURE.TRXTYPE_RENTAL_REQUEST, Text = "CONTAINER REQUEST", Value = "CONTAINER REQUEST" });
                doctypes.Add(new ResponseDocType { DocFeatureId = DOCNO_FEATURE.TRXTYPE_RENTAL_DELIVERY, Text = "CONTAINER DELIVERY", Value = "CONTAINER DELIVERY" });
                doctypes.Add(new ResponseDocType { DocFeatureId = DOCNO_FEATURE.TRXTYPE_RENTAL_CLOSING, Text = "CONTAINER CLOSING", Value = "CONTAINER CLOSING" });
            }
            else if (filter.TrxModule == TRX_MODULE.TRX_DEPOSIT)
            {
                doctypes.Add(new ResponseDocType { DocFeatureId = DOCNO_FEATURE.TRXTYPE_DEPOSIT_DEMURRAGE, Text = DOCUMENTTYPE.DEPOSIT_DEMURRAGE, Value = DOCUMENTTYPE.DEPOSIT_DEMURRAGE });
                doctypes.Add(new ResponseDocType { DocFeatureId = DOCNO_FEATURE.TRXTYPE_CONTAINER_GUARANTEE, Text = DOCUMENTTYPE.CONTAINER_GUARANTEE, Value = DOCUMENTTYPE.CONTAINER_GUARANTEE });
                doctypes.Add(new ResponseDocType { DocFeatureId = DOCNO_FEATURE.TRXTYPE_DETENTION, Text = DOCUMENTTYPE.DETENTION, Value = DOCUMENTTYPE.DETENTION });
            }

            return doctypes;
        }

        private IQueryable<ResponseItem> getTransactionType(string personId, RequestFilter filter)
        {
            var query = (from st in _context.FNDocNumberSetups
                         join tt in _context.MSTransactionTypes on st.TransactionType equals tt.TransactionType
                         where st.TrxModule == filter.TrxModule && !tt.InActive && !string.IsNullOrEmpty(st.DocNo)
                         select new ResponseItem {
                             DocFeatureId= st.DocFeatureId,
                             TransactionTypeId = tt.TransactionTypeId,
                             TransactionType = tt.TransactionType,
                             TransactionName = tt.TransactionName,
                             PaymentCondition = tt.PaymentCondition,
                             RequiredSubject = tt.RequiredSubject,                             
                             InActive = tt.InActive
                         }).Distinct().AsQueryable();

            var filterTransactionType = filter.TransactionType?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTransactionType.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterTransactionType)
                {
                    predicate = predicate.Or(x => x.TransactionType.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterTransactionName = filter.TransactionName?.Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (filterTransactionName.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterTransactionName)
                {
                    predicate = predicate.Or(x => x.TransactionName.Contains(filterItem, StringComparison.OrdinalIgnoreCase));
                }
                query = query.Where(predicate);
            }

            var filterPaymentCondition = filter.PaymentCondition?.Where(x => x.HasValue).ToList();
            if (filterPaymentCondition.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterPaymentCondition)
                {
                    predicate = predicate.Or(x => x.PaymentCondition == filterItem);
                }
                query = query.Where(predicate);
            }

            var filterRequiredSubject = filter.RequiredSubject?.Where(x => x.HasValue).ToList();
            if (filterRequiredSubject.Any())
            {
                var predicate = PredicateBuilder.New<ResponseItem>(true);
                foreach (var filterItem in filterRequiredSubject)
                {
                    predicate = predicate.Or(x => x.RequiredSubject == filterItem);
                }
                query = query.Where(predicate);
            }

            return query;
        }

        private IQueryable<ResponseItem> getTransactionTypeSorted(IQueryable<ResponseItem> input, List<string> sort)
        {
            var query = input.OrderBy(x => x.TransactionType);

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
