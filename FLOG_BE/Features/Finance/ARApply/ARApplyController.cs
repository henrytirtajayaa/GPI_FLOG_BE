using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Finance.ARApply
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class ARApplyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ARApplyController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        [Route("ApplyReceivable")]
        public async Task<IActionResult> CreateApplyReceivable([FromBody] PostApplyReceivable.RequestPaymentBody body)
        {
            var req = new PostApplyReceivable.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetProgressApplyReceivable")]
        public async Task<IActionResult> GetProgressApplyReceivable(
          
            [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> DocumentType
           , [CommaSeparated] List<string> CurrencyCode
           , [CommaSeparated] List<string> ReffDocumentNo
           , [CommaSeparated] List<decimal?> ExchangeRateMin
           , [CommaSeparated] List<decimal?> ExchangeRateMax
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
           , [CommaSeparated] int? Status
           , [CommaSeparated] List<string> StatusComment
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetProgressApplyReceivable.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetProgressApplyReceivable.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    DocumentType = DocumentType,
                    ReffDocumentNo = ReffDocumentNo,
                    CurrencyCode = CurrencyCode,
                    ExchangeRateMin = ExchangeRateMin,
                    ExchangeRateMax = ExchangeRateMax,
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
                    Status = Status,
                    StatusComment = StatusComment
                }
            };
            return ToActionResult(await _mediator.Send(req));
        } 
        

        [HttpGet]
        [Route("GetHistoryApplyReceivable")]
        public async Task<IActionResult> GetHistoryApplyReceivable(
          
            [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> DocumentType
           , [CommaSeparated] List<string> CurrencyCode
            , [CommaSeparated] List<string> ReffDocumentNo
           , [CommaSeparated] List<decimal?> ExchangeRateMin
           , [CommaSeparated] List<decimal?> ExchangeRateMax
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
           , [CommaSeparated] int? Status
           , [CommaSeparated] List<string> StatusComment
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetHistoryApplyReceivable.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetHistoryApplyReceivable.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    DocumentType = DocumentType,
                    CurrencyCode = CurrencyCode,
                    ReffDocumentNo = ReffDocumentNo,
                    ExchangeRateMin = ExchangeRateMin,
                    ExchangeRateMax = ExchangeRateMax,
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
                    Status = Status,
                    StatusComment = StatusComment
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("ApplyReceivable")]
        public async Task<IActionResult> UpdateApplyReceivable([FromBody] PutApplyReceivable.RequestPayment body)
        {
            var req = new PutApplyReceivable.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }


        [HttpDelete]
        [Route("ApplyReceivable")]
        public async Task<IActionResult> DeleteApplyReceivable([FromBody] DeleteApplyReceivable.RequestDeleteBody body)
        {
            var req = new DeleteApplyReceivable.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }


        [HttpPut]
        [Route("PutStatusApplyReceivable")]
        public async Task<IActionResult> PutStatusApplyReceivable([FromBody] PutStatusApplyReceivable.RequestPutStatus body)
        {
            var req = new PutStatusApplyReceivable.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

    }
}
