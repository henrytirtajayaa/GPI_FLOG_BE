using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Finance.ArReceipt
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class ArReceiptController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ArReceiptController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("CustomerReceipt")]
        public async Task<IActionResult> CreateCustomerReceipt([FromBody] PostCustomerReceipt.RequestCustomerReceiptBody body)
        {
            var req = new PostCustomerReceipt.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("CustomerReceipt")]
        public async Task<IActionResult> PutCustomerReceipt([FromBody] PutCustomerReceipt.RequestCustomerReceiptBody body)
        {
            var req = new PutCustomerReceipt.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetProgressCustomerReceipt")]
        public async Task<IActionResult> GetProgressCustomerReceipt(
            [CommaSeparated] List<DateTime?> TransactionDateStart 
            , [CommaSeparated] List<DateTime?> TransactionDateEnd 
            , [CommaSeparated] List<string> TransactionType 
            , [CommaSeparated] List<string> DocumentNo 
            , [CommaSeparated] List<string> CurrencyCode 
            , [CommaSeparated] List<decimal?> ExchangeRateMin 
            , [CommaSeparated] List<decimal?> ExchangeRateMax 
            , [CommaSeparated] List<string> CheckbookCode 
            , [CommaSeparated] Guid CustomerId 
            , [CommaSeparated] List<string> CustomerName 
            , [CommaSeparated] List<string> Description 
            , [CommaSeparated] List<decimal?> OriginatingTotalPaidMin 
            , [CommaSeparated] List<decimal?> OriginatingTotalPaidMax 
            , [CommaSeparated] List<decimal?> FunctionalTotalPaidMin 
            , [CommaSeparated] List<decimal?> FunctionalTotalPaidMax 
            , [CommaSeparated] List<string> CreatedBy 
            , [CommaSeparated] List<string> CreatedByName 
            , [CommaSeparated] List<DateTime?> CreatedDateStart 
            , [CommaSeparated] List<DateTime?> CreatedDateEnd 
            , [CommaSeparated] List<string> ModifiedBy 
            , [CommaSeparated] List<string> ModifiedByName 
            , [CommaSeparated] List<DateTime?> ModifiedDateStart 
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd 
            , [CommaSeparated] List<string> VoidBy 
            , [CommaSeparated] List<string> VoidByName 
            , [CommaSeparated] List<DateTime?> VoidDateStart 
            , [CommaSeparated] List<DateTime?> VoidDateEnd 
            , [CommaSeparated] int? Status 
            , [CommaSeparated] List<string> StatusComment 

           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetProgressCustomerReceipt.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetProgressCustomerReceipt.RequestFilter
                {
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    TransactionType = TransactionType,
                    DocumentNo = DocumentNo,
                    CurrencyCode = CurrencyCode,
                    ExchangeRateMin = ExchangeRateMin,
                    ExchangeRateMax = ExchangeRateMax,
                    CheckbookCode = CheckbookCode,
                    CustomerId = CustomerId,
                    CustomerName = CustomerName,
                    Description = Description,
                    OriginatingTotalPaidMin = OriginatingTotalPaidMin,
                    OriginatingTotalPaidMax = OriginatingTotalPaidMax,
                    FunctionalTotalPaidMin = FunctionalTotalPaidMin,
                    FunctionalTotalPaidMax = FunctionalTotalPaidMax,
                    CreatedBy = CreatedBy,
                    CreatedByName = CreatedByName,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    VoidBy = VoidBy,
                    VoidByName = VoidByName,
                    VoidDateStart = VoidDateStart,
                    VoidDateEnd = VoidDateEnd,
                    Status = Status,
                    StatusComment = StatusComment
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetHistoryCustomerReceipt")]
        public async Task<IActionResult> GetHistoryCustomerReceipt(
            [CommaSeparated] List<DateTime?> TransactionDateStart
            , [CommaSeparated] List<DateTime?> TransactionDateEnd
            , [CommaSeparated] List<string> TransactionType
            , [CommaSeparated] List<string> DocumentNo
            , [CommaSeparated] List<string> CurrencyCode
            , [CommaSeparated] List<decimal?> ExchangeRateMin
            , [CommaSeparated] List<decimal?> ExchangeRateMax
            , [CommaSeparated] List<string> CheckbookCode
            , [CommaSeparated] Guid CustomerId
            , [CommaSeparated] List<string> CustomerName
            , [CommaSeparated] List<string> Description
            , [CommaSeparated] List<decimal?> OriginatingTotalPaidMin
            , [CommaSeparated] List<decimal?> OriginatingTotalPaidMax
            , [CommaSeparated] List<decimal?> FunctionalTotalPaidMin
            , [CommaSeparated] List<decimal?> FunctionalTotalPaidMax
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<string> CreatedByName
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> ModifiedBy
            , [CommaSeparated] List<string> ModifiedByName
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd
            , [CommaSeparated] List<string> VoidBy
            , [CommaSeparated] List<string> VoidByName
            , [CommaSeparated] List<DateTime?> VoidDateStart
            , [CommaSeparated] List<DateTime?> VoidDateEnd
            , [CommaSeparated] int? Status
            , [CommaSeparated] List<string> StatusComment

           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetHistoryCustomerReceipt.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetHistoryCustomerReceipt.RequestFilter
                {
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    TransactionType = TransactionType,
                    DocumentNo = DocumentNo,
                    CurrencyCode = CurrencyCode,
                    ExchangeRateMin = ExchangeRateMin,
                    ExchangeRateMax = ExchangeRateMax,
                    CheckbookCode = CheckbookCode,
                    CustomerId = CustomerId,
                    CustomerName = CustomerName,
                    Description = Description,
                    OriginatingTotalPaidMin = OriginatingTotalPaidMin,
                    OriginatingTotalPaidMax = OriginatingTotalPaidMax,
                    FunctionalTotalPaidMin = FunctionalTotalPaidMin,
                    FunctionalTotalPaidMax = FunctionalTotalPaidMax,
                    CreatedBy = CreatedBy,
                    CreatedByName = CreatedByName,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    VoidBy = VoidBy,
                    VoidByName = VoidByName,
                    VoidDateStart = VoidDateStart,
                    VoidDateEnd = VoidDateEnd,
                    Status = Status,
                    StatusComment = StatusComment
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetDetailCustomerReceipt")]
        public async Task<IActionResult> GetDetailCustomerReceipt(
           Guid ReceiptHeaderId
           ,Guid ReceiveTransactionId
           , [CommaSeparated] string DocumnetNo
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetDetailCustomerReceipt.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetDetailCustomerReceipt.RequestFilter
                {
                    ReceiptHeaderId = ReceiptHeaderId,
                    ReceiveTransactionId = ReceiveTransactionId,
                    DocumentNo = DocumnetNo
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("DeleteCustomerReceipt")]
        public async Task<IActionResult> DeleteCustomerReceipt ([FromBody] DeleteCustomerReceipt.RequestDeleteCustomerReceipt body)
        {
            var req = new DeleteCustomerReceipt.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutStatusCustomerReceipt")]
        public async Task<IActionResult> PutStatusPayableTransaction([FromBody] PutStatusCustomerReceipt.RequestPutStatus body)
        {
            var req = new PutStatusCustomerReceipt.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetCustomerReceiptById")]
        public async Task<IActionResult> GetReceiveById(
            [CommaSeparated] Guid ReceiptHeaderId)
        {
            var req = new GetCustomerReceiptById.Request
            {
                ReceiptHeaderId = ReceiptHeaderId
            };
            return ToActionResult(await _mediator.Send(req));
        }

    }
}
