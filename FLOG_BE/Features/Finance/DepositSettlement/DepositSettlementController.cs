using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Finance.DepositSettlement
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class DepositSettlementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DepositSettlementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("DepositSettlement")]
        public async Task<IActionResult> CreateDepositSettlement([FromBody] PostDepositSettlement.RequestDepositSettlement body)
        {
            var req = new PostDepositSettlement.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("DepositSettlement")]
        public async Task<IActionResult> PutDepositSettlement([FromBody] PutDepositSettlement.RequestDepositSettlement body)
        {
            var req = new PutDepositSettlement.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutStatusDepositSettlement")]
        public async Task<IActionResult> PutStatusDepositSettlement([FromBody] PutStatusDepositSettlement.RequestPutStatus body)
        {
            var req = new PutStatusDepositSettlement.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("DepositSettlement")]
        public async Task<IActionResult> DeleteDepositSettlement([FromBody] DeleteDepositSettlement.RequestDeleteDepositSettlement body)
        {
            var req = new DeleteDepositSettlement.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetProgressDepositSettlement")]
        public async Task<IActionResult> GetProgressDepositSettlement(
            [CommaSeparated] List<DateTime?> TransactionDateStart
            , [CommaSeparated] List<DateTime?> TransactionDateEnd
            , [CommaSeparated] List<string> DocumentType
            , [CommaSeparated] List<string> DocumentNo
            , [CommaSeparated] List<string> DepositNo
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
            var req = new GetProgress.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetProgress.RequestFilter
                {
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    DocumentType = DocumentType,
                    DocumentNo = DocumentNo,
                    DepositNo = DepositNo,
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
        [Route("GetHistoryDepositSettlement")]
        public async Task<IActionResult> GetHistoryDepositSettlement(
            [CommaSeparated] List<DateTime?> TransactionDateStart
            , [CommaSeparated] List<DateTime?> TransactionDateEnd
            , [CommaSeparated] List<string> DocumentType
            , [CommaSeparated] List<string> DocumentNo
            , [CommaSeparated] List<string> DepositNo
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
            var req = new GetHistory.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetHistory.RequestFilter
                {
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    DocumentType = DocumentType,
                    DocumentNo = DocumentNo,
                    DepositNo = DepositNo,
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
        [Route("GetDetailDepositSettlement")]
        public async Task<IActionResult> GetDetailDepositSettlement(
           Guid SettlementHeaderId
           , Guid ReceiveTransactionId
           , [CommaSeparated] string DocumnetNo
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetDetailDepositSettlement.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetDetailDepositSettlement.RequestFilter
                {
                    SettlementHeaderId = SettlementHeaderId,
                    ReceiveTransactionId = ReceiveTransactionId,
                    DocumentNo = DocumnetNo
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetPendingDeposit")]
        public async Task<IActionResult> GetPendingDeposit(
          Guid CustomerId
          , string CurrencyCode
          , string DocumentNo
          , string NSDocumentNo
          , DateTime? TransactionDateStart
          , DateTime? TransactionDateEnd
          , [CommaSeparated] List<string> sort
          , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetPendingDeposit.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetPendingDeposit.RequestFilter
                {
                    CustomerId = CustomerId,
                    CurrencyCode = CurrencyCode,
                    DocumentNo = DocumentNo,
                    NSDocumentNo = NSDocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetUnapplyDeposit")]
        public async Task<IActionResult> GetUnapplyDeposit(
           Guid CustomerId
           , string CurrencyCode
           , string DocumentNo
           , DateTime? TransactionDateStart
           , DateTime? TransactionDateEnd
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetUnapplyDeposit.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetUnapplyDeposit.RequestFilter
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
        [Route("GetDepositSettlementById")]
        public async Task<IActionResult> GetDepositSettlementById(
            [CommaSeparated] Guid SettlementHeaderId)
        {
            var req = new GetDepositSettlementById.Request
            {
                SettlementHeaderId = SettlementHeaderId
            };
            return ToActionResult(await _mediator.Send(req));
        }

    }
}
