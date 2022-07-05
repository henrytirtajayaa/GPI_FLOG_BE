using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Finance.APApply
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class APApplyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public APApplyController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        [Route("ApplyPayable")]
        public async Task<IActionResult> CreateApplyPayable([FromBody] PostApplyPayable.RequestPaymentBody body)
        {
            var req = new PostApplyPayable.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetProgressApplyPayable")]
        public async Task<IActionResult> GetProgressApplyPayable(
          
            [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> DocumentType
           , [CommaSeparated] List<string> CurrencyCode
           , [CommaSeparated] List<string> ReffDocumentNo
           , [CommaSeparated] List<decimal?> ExchangeRateMin
           , [CommaSeparated] List<decimal?> ExchangeRateMax
           , [CommaSeparated] List<string> VendorName
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
            var req = new GetProgressApplyPayable.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetProgressApplyPayable.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    DocumentType = DocumentType,
                    ReffDocumentNo = ReffDocumentNo,
                    CurrencyCode = CurrencyCode,
                    ExchangeRateMin = ExchangeRateMin,
                    ExchangeRateMax = ExchangeRateMax,
                    VendorName = VendorName,
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
        [Route("GetHistoryApplyPayable")]
        public async Task<IActionResult> GetHistoryApplyPayable(
          
            [CommaSeparated] List<string> DocumentNo
           , [CommaSeparated] List<DateTime?> TransactionDateStart
           , [CommaSeparated] List<DateTime?> TransactionDateEnd
           , [CommaSeparated] List<string> DocumentType
           , [CommaSeparated] List<string> CurrencyCode
            , [CommaSeparated] List<string> ReffDocumentNo
           , [CommaSeparated] List<decimal?> ExchangeRateMin
           , [CommaSeparated] List<decimal?> ExchangeRateMax
           , [CommaSeparated] List<string> VendorName
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
            var req = new GetHistoryApplyPayable.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetHistoryApplyPayable.RequestFilter
                {
                    DocumentNo = DocumentNo,
                    TransactionDateStart = TransactionDateStart,
                    TransactionDateEnd = TransactionDateEnd,
                    DocumentType = DocumentType,
                    CurrencyCode = CurrencyCode,
                    ReffDocumentNo = ReffDocumentNo,
                    ExchangeRateMin = ExchangeRateMin,
                    ExchangeRateMax = ExchangeRateMax,
                    VendorName = VendorName,
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
        [Route("ApplyPayable")]
        public async Task<IActionResult> UpdateApplyPayable([FromBody] PutApplyPayable.RequestPayment body)
        {
            var req = new PutApplyPayable.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
        [HttpDelete]
        [Route("ApplyPayable")]
        public async Task<IActionResult> DeleteApplyPayable([FromBody] DeleteApplyPayable.RequestDeleteBody body)
        {
            var req = new DeleteApplyPayable.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }


        [HttpPut]
        [Route("PutStatusApplyPayable")]
        public async Task<IActionResult> PutStatusApplyPayable([FromBody] PutStatusApplyPayable.RequestPutStatus body)
        {
            var req = new PutStatusApplyPayable.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

    }
}
