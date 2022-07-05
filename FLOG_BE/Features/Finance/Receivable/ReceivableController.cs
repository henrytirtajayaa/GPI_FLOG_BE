using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Finance.Receivable
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class ReceivableController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReceivableController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("ReceivableTransaction")]
        public async Task<IActionResult> CreateVendor([FromBody] PostReceivableTransaction.RequestReceivableBody body)
        {
            var req = new PostReceivableTransaction.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
        [HttpGet]
        [Route("GetHistoryReceivable")]
        public async Task<IActionResult> GetHistory(
              [CommaSeparated] List<string> DocumentType
             , [CommaSeparated] List<string> DocumentNo
             , [CommaSeparated] List<string> BranchCode
             , [CommaSeparated] List<DateTime?> TransactionDateStart
             , [CommaSeparated] List<DateTime?> TransactionDateEnd
             , [CommaSeparated] List<string> TransactionType
             , [CommaSeparated] List<string> CurrencyCode
             , [CommaSeparated] List<decimal?> ExchangeRateMin
             , [CommaSeparated] List<decimal?> ExchangeRateMax
             , [CommaSeparated] List<string> CustomerName
             , [CommaSeparated] Guid CustomerId
             , [CommaSeparated] List<string> PaymentTermCode
             , [CommaSeparated] List<string> CustomerAddressCode
             , [CommaSeparated] List<string> SoDocumentNo
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
             , [CommaSeparated] List<string> CreatedName
             , [CommaSeparated] List<DateTime?> CreatedDateStart
             , [CommaSeparated] List<DateTime?> CreatedDateEnd
             , [CommaSeparated] List<string> ModifiedBy
             , [CommaSeparated] List<string> ModifiedByName
             , [CommaSeparated] List<DateTime?> ModifiedDateStart
             , [CommaSeparated] List<DateTime?> ModifiedDateEnd
             , [CommaSeparated] List<DateTime?> ModifiedDate
             , [CommaSeparated] int? Status
             , [CommaSeparated] List<string> StatusComment
             , [CommaSeparated] List<string> BillToAddressCode
             , [CommaSeparated] List<string> ShipToAddressCode
             , [CommaSeparated] List<decimal?> OriginatingExtendedAmountMin
             , [CommaSeparated] List<decimal?> OriginatingExtendedAmountMax
             , [CommaSeparated] List<string> sort
             , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetHistoryReceivable.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetHistoryReceivable.RequestFilter
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
                    CustomerId = CustomerId,
                    CustomerName = CustomerName,
                    PaymentTermCode = PaymentTermCode,
                    CustomerAddressCode = CustomerAddressCode,
                    SoDocumentNo = SoDocumentNo,
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
                    CreatedName = CreatedName,
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

        [HttpPut]
        [Route("ReceivableTransaction")]
        public async Task<IActionResult> PutReceivable([FromBody] PutReceivableTransaction.RequestReceivable body)
        {
            var req = new PutReceivableTransaction.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetProgressReceivable")]
        public async Task<IActionResult> GetProgress(
            [CommaSeparated] List<string> DocumentType
           , [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<string> BranchCode
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> TransactionType
           , [CommaSeparated] List<string> CurrencyCode
           , [CommaSeparated] List<decimal?> ExchangeRateMin
           , [CommaSeparated] List<decimal?> ExchangeRateMax
           , [CommaSeparated] List<string> CustomerName
           , [CommaSeparated] List<string> PaymentTermCode
           , [CommaSeparated] List<string> CustomerAddressCode
           , [CommaSeparated] List<string> SoDocumentNo
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
           , [CommaSeparated] List<string> CreatedName
           , [CommaSeparated] List<DateTime?> CreatedDateStart
           , [CommaSeparated] List<DateTime?> CreatedDateEnd
           , [CommaSeparated] List<string> ModifiedBy
           , [CommaSeparated] List<string> ModifiedByName
           , [CommaSeparated] List<DateTime?> ModifiedDateStart
           , [CommaSeparated] List<DateTime?> ModifiedDateEnd
           , [CommaSeparated] List<DateTime?> ModifiedDate
           , [CommaSeparated] int? Status
           , [CommaSeparated] List<string> StatusComment
           , [CommaSeparated] List<string> BillToAddressCode
           , [CommaSeparated] List<string> ShipToAddressCode
           , [CommaSeparated] List<decimal?> OriginatingExtendedAmountMin
           , [CommaSeparated] List<decimal?> OriginatingExtendedAmountMax

           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetProgressReceivable.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetProgressReceivable.RequestFilter
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
                    CustomerName = CustomerName,
                    PaymentTermCode = PaymentTermCode,
                    CustomerAddressCode = CustomerAddressCode,
                    SoDocumentNo = SoDocumentNo,
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
                    CreatedName = CreatedName,
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

        [HttpPut]
        [Route("PutTrxDeleteReceivable")]
        public async Task<IActionResult> PutTrxDeleteReceivable([FromBody] PutTrxDelete.RequestTransactionDeleteBody body)
        {
            var req = new PutTrxDelete.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
        [HttpPut]
        [Route("PutStatusReceivable")]
        public async Task<IActionResult> PutPostingReceivable([FromBody] PutReceivableTransaction.RequestReceivable body)
        {
            var req = new PutStatusReceivable.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutStatusDeposit")]
        public async Task<IActionResult> PutPostingDeposit([FromBody] PutReceivableTransaction.RequestReceivable body)
        {
            var req = new PutStatusDeposit.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetPendingReceivable")]
        public async Task<IActionResult> GetPendingReceivable(
          Guid CustomerId
          , string CurrencyCode
          , string DocumentNo
          , string NSDocumentNo
          , string MasterNo
          , string AgreementNo
          , DateTime? TransactionDateStart
          , DateTime? TransactionDateEnd
          , [CommaSeparated] List<string> sort
          , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetPendingReceivable.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetPendingReceivable.RequestFilter
                {
                    CustomerId = CustomerId,
                    CurrencyCode = CurrencyCode,
                    DocumentNo = DocumentNo,
                    NSDocumentNo = NSDocumentNo,
                    MasterNo = MasterNo,
                    AgreementNo = AgreementNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetPendingAdvanceReceipt")]
        public async Task<IActionResult> GetPendingAdvanceReceipt(
           Guid CustomerId
           , string CurrencyCode
           , string DocumentNo
           , string CheckbookCode
           , DateTime? TransactionDateStart
           , DateTime? TransactionDateEnd
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetAdvanceReceipt.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetAdvanceReceipt.RequestFilter
                {
                    CustomerId = CustomerId,
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
        [Route("GetPendingARCreditNote")]
        public async Task<IActionResult> GetPendingCreditNote(
           Guid CustomerId
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
                    CustomerId = CustomerId,
                    CurrencyCode = CurrencyCode,
                    DocumentNo = DocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetReceivableTransactionById")]
        public async Task<IActionResult> GetReceiveById(
            [CommaSeparated] Guid ReceiveTransactionId)
        {
            var req = new GetReceivableTransactionById.Request
            {
                ReceiveTransactionId = ReceiveTransactionId
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetProgressDeposit")]
        public async Task<IActionResult> GetProgressDeposit(
            [CommaSeparated] List<string> DocumentType
           , [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<string> BranchCode
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> TransactionType
           , [CommaSeparated] List<string> CurrencyCode
           , [CommaSeparated] List<decimal?> ExchangeRateMin
           , [CommaSeparated] List<decimal?> ExchangeRateMax
           , [CommaSeparated] List<string> CustomerName
           , [CommaSeparated] List<string> PaymentTermCode
           , [CommaSeparated] List<string> CustomerAddressCode
           , [CommaSeparated] List<string> SoDocumentNo
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
           , [CommaSeparated] List<string> CreatedName
           , [CommaSeparated] List<DateTime?> CreatedDateStart
           , [CommaSeparated] List<DateTime?> CreatedDateEnd
           , [CommaSeparated] List<string> ModifiedBy
           , [CommaSeparated] List<string> ModifiedByName
           , [CommaSeparated] List<DateTime?> ModifiedDateStart
           , [CommaSeparated] List<DateTime?> ModifiedDateEnd
           , [CommaSeparated] List<DateTime?> ModifiedDate
           , [CommaSeparated] int? Status
           , [CommaSeparated] List<string> StatusComment
           , [CommaSeparated] List<string> BillToAddressCode
           , [CommaSeparated] List<string> ShipToAddressCode
           , [CommaSeparated] List<decimal?> OriginatingExtendedAmountMin
           , [CommaSeparated] List<decimal?> OriginatingExtendedAmountMax

           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetProgressDeposit.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetProgressDeposit.RequestFilter
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
                    CustomerName = CustomerName,
                    PaymentTermCode = PaymentTermCode,
                    CustomerAddressCode = CustomerAddressCode,
                    SoDocumentNo = SoDocumentNo,
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
                    CreatedName = CreatedName,
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
        [Route("GetHistoryDeposit")]
        public async Task<IActionResult> GetHistoryDeposit(
              [CommaSeparated] List<string> DocumentType
             , [CommaSeparated] List<string> DocumentNo
             , [CommaSeparated] List<string> BranchCode
             , [CommaSeparated] List<DateTime?> TransactionDateStart
             , [CommaSeparated] List<DateTime?> TransactionDateEnd
             , [CommaSeparated] List<string> TransactionType
             , [CommaSeparated] List<string> CurrencyCode
             , [CommaSeparated] List<decimal?> ExchangeRateMin
             , [CommaSeparated] List<decimal?> ExchangeRateMax
             , [CommaSeparated] List<string> CustomerName
             , [CommaSeparated] Guid CustomerId
             , [CommaSeparated] List<string> PaymentTermCode
             , [CommaSeparated] List<string> CustomerAddressCode
             , [CommaSeparated] List<string> SoDocumentNo
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
             , [CommaSeparated] List<string> CreatedName
             , [CommaSeparated] List<DateTime?> CreatedDateStart
             , [CommaSeparated] List<DateTime?> CreatedDateEnd
             , [CommaSeparated] List<string> ModifiedBy
             , [CommaSeparated] List<string> ModifiedByName
             , [CommaSeparated] List<DateTime?> ModifiedDateStart
             , [CommaSeparated] List<DateTime?> ModifiedDateEnd
             , [CommaSeparated] List<DateTime?> ModifiedDate
             , [CommaSeparated] int? Status
             , [CommaSeparated] List<string> StatusComment
             , [CommaSeparated] List<string> BillToAddressCode
             , [CommaSeparated] List<string> ShipToAddressCode
             , [CommaSeparated] List<decimal?> OriginatingExtendedAmountMin
             , [CommaSeparated] List<decimal?> OriginatingExtendedAmountMax
             , [CommaSeparated] List<string> sort
             , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetHistoryDeposit.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetHistoryDeposit.RequestFilter
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
                    CustomerId = CustomerId,
                    CustomerName = CustomerName,
                    PaymentTermCode = PaymentTermCode,
                    CustomerAddressCode = CustomerAddressCode,
                    SoDocumentNo = SoDocumentNo,
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
                    CreatedName = CreatedName,
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
        [Route("GetPendingARUnapply")]
        public async Task<IActionResult> GetPendingARUnapply(
           Guid CustomerId
           , string CurrencyCode
           , string DocumentNo
           , DateTime? TransactionDateStart
           , DateTime? TransactionDateEnd
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetUnapplyReceipt.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetUnapplyReceipt.RequestFilter
                    {
                        CustomerId = CustomerId,
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
