using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Finance.Payable
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PayableController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PayableController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetProgressPayable")]
        public async Task<IActionResult> GetProgressPayable(
            [CommaSeparated] List<string> DocumentType
           , [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<string> BranchCode
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> TransactionType
           , [CommaSeparated] List<string> CurrencyCode
           , [CommaSeparated] List<decimal?> ExchangeRateMin
           , [CommaSeparated] List<decimal?> ExchangeRateMax
           , [CommaSeparated] List<string> VendorName
           , [CommaSeparated] Guid VendorId
           , [CommaSeparated] List<string> PaymentTermCode
           , [CommaSeparated] List<string> VendorAddressCode
           , [CommaSeparated] List<string> VendorDocumentNo
           , [CommaSeparated] List<string> NsDocumentNo
           , [CommaSeparated] List<string> MasterNo
           , [CommaSeparated] List<string> AgreementNo
           , [CommaSeparated] List<string> Description
           , [CommaSeparated] List<decimal?> SubtotalAmountMin
           , [CommaSeparated] List<decimal?> SubtotalAmountMax
           , [CommaSeparated] List<decimal?> DiscountAmountMin
           , [CommaSeparated] List<decimal?> DiscountAmountMax
           , [CommaSeparated] List<decimal?> TaxAmountMin
           , [CommaSeparated] List<decimal?> TaxAmountMax
           , [CommaSeparated] List<string> VoidBy
           , [CommaSeparated] List<DateTime?> VoidDateStart
           , [CommaSeparated] List<DateTime?> VoidDateEnd
           , [CommaSeparated] List<string> CreatedBy
           , [CommaSeparated] List<string> CreatedByName
           , [CommaSeparated] List<DateTime?> CreatedDateStart
           , [CommaSeparated] List<DateTime?> CreatedDateEnd
           , [CommaSeparated] List<string> ModifiedBy
           , [CommaSeparated] List<string> ModifiedByName
           , [CommaSeparated] List<DateTime?> ModifiedDateStart
           , [CommaSeparated] List<DateTime?> ModifiedDateEnd
           , [CommaSeparated] int? Status
           , [CommaSeparated] List<string> StatusComment
           , [CommaSeparated] List<string> BillToAddressCode
           , [CommaSeparated] List<string> ShipToAddressCode
           , [CommaSeparated] List<decimal?> OriginatingExtendedAmountMin
           , [CommaSeparated] List<decimal?> OriginatingExtendedAmountMax

           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetProgress.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetProgress.RequestFilter
                {
                    DocumentType = DocumentType,
                    DocumentNo = DocumentNo,
                    BranchCode = BranchCode,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    TransactionType = TransactionType,
                    CurrencyCode = CurrencyCode,
                    ExchangeRateMin = ExchangeRateMin,
                    ExchangeRateMax = ExchangeRateMax,
                    VendorName = VendorName,
                    VendorId = VendorId,
                    PaymentTermCode = PaymentTermCode,
                    VendorAddressCode = VendorAddressCode,
                    VendorDocumentNo = VendorDocumentNo,
                    NsDocumentNo = NsDocumentNo,
                    MasterNo = MasterNo,
                    AgreementNo = AgreementNo,
                    Description = Description,
                    SubtotalAmountMin = SubtotalAmountMin,
                    SubtotalAmountMax = SubtotalAmountMax,
                    DiscountAmountMin = DiscountAmountMin,
                    DiscountAmountMax = DiscountAmountMax,
                    TaxAmountMin = TaxAmountMin,
                    TaxAmountMax = TaxAmountMax,
                    VoidBy = VoidBy,
                    VoidDateStart = VoidDateStart,
                    VoidDateEnd = VoidDateEnd,
                    CreatedBy = CreatedBy,
                    CreatedByName = CreatedByName,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    Status = Status,
                    StatusComment = StatusComment,
                    BillToAddressCode = BillToAddressCode,
                    ShipToAddressCode = ShipToAddressCode,
                    OriginatingExtendedAmountMin = OriginatingExtendedAmountMin,
                    OriginatingExtendedAmountMax = OriginatingExtendedAmountMax
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetHistoryPayable")]
        public async Task<IActionResult> GetHistoryPayable(
            [CommaSeparated] List<string> DocumentType
           , [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<string> BranchCode
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> TransactionType
           , [CommaSeparated] List<string> CurrencyCode
           , [CommaSeparated] List<decimal?> ExchangeRateMin
           , [CommaSeparated] List<decimal?> ExchangeRateMax
           , [CommaSeparated] List<string> VendorName
           , [CommaSeparated] Guid VendorId
           , [CommaSeparated] List<string> PaymentTermCode
           , [CommaSeparated] List<string> VendorAddressCode
           , [CommaSeparated] List<string> VendorDocumentNo
           , [CommaSeparated] List<string> NsDocumentNo
           , [CommaSeparated] List<string> MasterNo
           , [CommaSeparated] List<string> AgreementNo
           , [CommaSeparated] List<string> Description
           , [CommaSeparated] List<decimal?> SubtotalAmountMin
           , [CommaSeparated] List<decimal?> SubtotalAmountMax
           , [CommaSeparated] List<decimal?> DiscountAmountMin
           , [CommaSeparated] List<decimal?> DiscountAmountMax
           , [CommaSeparated] List<decimal?> TaxAmountMin
           , [CommaSeparated] List<decimal?> TaxAmountMax
           , [CommaSeparated] List<string> VoidBy
           , [CommaSeparated] List<DateTime?> VoidDateStart
           , [CommaSeparated] List<DateTime?> VoidDateEnd
           , [CommaSeparated] List<string> CreatedBy
           , [CommaSeparated] List<string> CreatedByName
           , [CommaSeparated] List<DateTime?> CreatedDateStart
           , [CommaSeparated] List<DateTime?> CreatedDateEnd
           , [CommaSeparated] List<string> ModifiedBy
           , [CommaSeparated] List<string> ModifiedByName
           , [CommaSeparated] List<DateTime?> ModifiedDateStart
           , [CommaSeparated] List<DateTime?> ModifiedDateEnd
           , [CommaSeparated] int? Status
           , [CommaSeparated] List<string> StatusComment
           , [CommaSeparated] List<string> BillToAddressCode
           , [CommaSeparated] List<string> ShipToAddressCode
           , [CommaSeparated] List<decimal?> OriginatingExtendedAmountMin
           , [CommaSeparated] List<decimal?> OriginatingExtendedAmountMax

           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetHistory.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetHistory.RequestFilter
                {
                    DocumentType = DocumentType,
                    DocumentNo = DocumentNo,
                    BranchCode = BranchCode,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    TransactionType = TransactionType,
                    CurrencyCode = CurrencyCode,
                    ExchangeRateMin = ExchangeRateMin,
                    ExchangeRateMax = ExchangeRateMax,
                    VendorId = VendorId,
                    VendorName = VendorName,
                    PaymentTermCode = PaymentTermCode,
                    VendorAddressCode = VendorAddressCode,
                    VendorDocumentNo = VendorDocumentNo,
                    NsDocumentNo = NsDocumentNo,
                    MasterNo = MasterNo,
                    AgreementNo = AgreementNo,
                    Description = Description,
                    SubtotalAmountMin = SubtotalAmountMin,
                    SubtotalAmountMax = SubtotalAmountMax,
                    DiscountAmountMin = DiscountAmountMin,
                    DiscountAmountMax = DiscountAmountMax,
                    TaxAmountMin = TaxAmountMin,
                    TaxAmountMax = TaxAmountMax,
                    VoidBy = VoidBy,
                    VoidDateStart = VoidDateStart,
                    VoidDateEnd = VoidDateEnd,
                    CreatedBy = CreatedBy,
                    CreatedByName = CreatedByName,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    Status = Status,
                    StatusComment = StatusComment,
                    BillToAddressCode = BillToAddressCode,
                    ShipToAddressCode = ShipToAddressCode,
                    OriginatingExtendedAmountMin = OriginatingExtendedAmountMin,
                    OriginatingExtendedAmountMax = OriginatingExtendedAmountMax
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("PayableTransaction")]
        public async Task<IActionResult> CreatePayableHeader([FromBody] PostPayable.RequestPayableBody body)
        {
            var req = new PostPayable.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PayableTransaction")]
        public async Task<IActionResult> UpdatePayableHeader([FromBody] PutPayable.RequestPayable body)
        {
            var req = new PutPayable.Request
            { 
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutStatusPayable")]
        public async Task<IActionResult> PutStatusPayableTransaction([FromBody] PutPayable.RequestPayable body)
        {
            var req = new PutStatusPayable.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutTrxDeletePayable")]
        public async Task<IActionResult> PutTrxDeletePayable([FromBody] PutTrxDeletePayable.RequestTransactionDeletePayable body)
        {
            var req = new PutTrxDeletePayable.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetPendingPayable")]
        public async Task<IActionResult> GetPendingPayableInvoices(
           Guid VendorId
           , string CurrencyCode
           , string DocumentNo
           , string MasterNo
           , string AgreementNo
           , string VendorDocNo
           , string NsDocumentNo
           , DateTime? TransactionDateStart
           , DateTime? TransactionDateEnd
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetPendingPayable.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetPendingPayable.RequestFilter
                {
                    VendorId = VendorId,     
                    CurrencyCode = CurrencyCode,
                    DocumentNo = DocumentNo,
                    MasterNo = MasterNo,
                    AgreementNo = AgreementNo,
                    VendorDocumentNo = VendorDocNo,
                    NsDocumentNo = NsDocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetPendingAdvancePayment")]
        public async Task<IActionResult> GetPendingAdvancePayment(
           Guid VendorId
           , string CurrencyCode
           , string DocumentNo
           , string CheckbookCode
           , DateTime? TransactionDateStart
           , DateTime? TransactionDateEnd
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetAdvancePayment.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetAdvancePayment.RequestFilter
                {
                    VendorId = VendorId,
                    CurrencyCode = CurrencyCode,
                    DocumentNo = DocumentNo,
                    CheckbookCode = CheckbookCode,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetPendingAPCreditNote")]
        public async Task<IActionResult> GetPendingCreditNote(
           Guid VendorId
           , string CurrencyCode
           , string DocumentNo
           , DateTime? TransactionDateStart
           , DateTime? TransactionDateEnd
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetCreditNote.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetCreditNote.RequestFilter
                {
                    VendorId = VendorId,
                    CurrencyCode = CurrencyCode,
                    DocumentNo = DocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetPayableTransactionById")]
        public async Task<IActionResult> GetPayableById(
            [CommaSeparated] Guid PayableTransactionId)
        {
            var req = new GetPayableTransactionById.Request
            {
                PayableTransactionId = PayableTransactionId
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetPendingAPUnapply")]
        public async Task<IActionResult> GetPendingAPUnapply(
           Guid VendorId
           , string CurrencyCode
           , string DocumentNo
           , DateTime? TransactionDateStart
           , DateTime? TransactionDateEnd
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetUnapplyPayment.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetUnapplyPayment.RequestFilter
                {
                    VendorId = VendorId,
                    CurrencyCode = CurrencyCode,
                    DocumentNo = DocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
