using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.TaxSchedule
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class TaxScheduleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaxScheduleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("TaxSchedule")]
        public async Task<IActionResult> GetTaxSchedule(
            [CommaSeparated] List<string> TaxScheduleCode
            , [CommaSeparated] List<string> Description
            , [CommaSeparated] List<bool?> IsSales
            , [CommaSeparated] List<decimal?> PercentOfSalesPurchaseMin
            , [CommaSeparated] List<decimal?> PercentOfSalesPurchaseMax
            , [CommaSeparated] List<decimal?> TaxablePercentMin
            , [CommaSeparated] List<decimal?> TaxablePercentMax
            , [CommaSeparated] List<byte?> RoundingType
            , [CommaSeparated] List<decimal?> RoundingLimitAmountMin
            , [CommaSeparated] List<decimal?> RoundingLimitAmountMax
            , [CommaSeparated] List<bool?> TaxInclude
            , [CommaSeparated] List<bool?> WithHoldingTax
            , [CommaSeparated] List<string> TaxAccountNo
            , [CommaSeparated] List<bool?> Inactive
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> ModifiedBy
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetTaxSchedule.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetTaxSchedule.RequestFilter
                {
                    TaxScheduleCode = TaxScheduleCode,
                    Description = Description,
                    IsSales = IsSales,
                    PercentOfSalesPurchaseMin = PercentOfSalesPurchaseMin,
                    PercentOfSalesPurchaseMax = PercentOfSalesPurchaseMax,
                    TaxablePercentMin = TaxablePercentMin,
                    TaxablePercentMax = TaxablePercentMax,
                    RoundingType = RoundingType,
                    RoundingLimitAmountMin = RoundingLimitAmountMin,
                    RoundingLimitAmountMax = RoundingLimitAmountMax,
                    TaxInclude = TaxInclude,
                    WithHoldingTax = WithHoldingTax,
                    TaxAccountNo = TaxAccountNo,
                    Inactive = Inactive,
                    CreatedBy = CreatedBy,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("TaxSchedule")]
        public async Task<IActionResult> CreateTaxSchedule([FromBody] PostTaxSchedule.RequestTaxScheduleBody body)
        {
           var req = new PostTaxSchedule.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("TaxSchedule")]
        public async Task<IActionResult> DeleteTaxSchedule(DeleteTaxSchedule.RequestTaxScheduleDelete body)
        {
            var req = new DeleteTaxSchedule.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("TaxSchedule")]
        public async Task<IActionResult> UpdateTaxSchedule([FromBody] PutTaxSchedule.RequestPutTaxSheduleBody body)
        {
            var req = new PutTaxSchedule.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
