using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.ExchangeRateHeader
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ExchangeRateHeaderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExchangeRateHeaderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("ExchangeRate")]
        public async Task<IActionResult> CreateExchangeRate([FromBody] PostExchangeRateHeader.RequestExchangeRateHeader body)
        {
            var req = new PostExchangeRateHeader.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("ExchangeRate")]
        public async Task<IActionResult> UpdateExchangeRate([FromBody] PutExchangeRateHeader.RequestExchangeRateHeader body)
        {
            var req = new PutExchangeRateHeader.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("ExchangeRate")]
        public async Task<IActionResult> GetExchangeRate([CommaSeparated] List<string> ExchangeRateCode
             , [CommaSeparated] List<string> Description
             , [CommaSeparated] List<string> CurrencyCode
             , [CommaSeparated] List<int?> RateTypeMin
             , [CommaSeparated] List<int?> RateTypeMax
             , [CommaSeparated] List<string> ExpiredPeriod
             , [CommaSeparated] List<int?> CalculationTypeMin
             , [CommaSeparated] List<int?> CalculationTypeMax
             , [CommaSeparated] List<int?> StatusMin
             , [CommaSeparated] List<int?> StatusMax
             , [CommaSeparated] List<string> CreatedBy
             , [CommaSeparated] List<string> CreatedByName
             , [CommaSeparated] List<DateTime?> CreatedDateStart
             , [CommaSeparated] List<DateTime?> CreatedDateEnd
             , [CommaSeparated] List<DateTime?> ModifiedDateStart
             , [CommaSeparated] List<DateTime?> ModifiedDateEnd
             , [CommaSeparated] List<string> ModifiedBy
             , [CommaSeparated] List<DateTime?> ModifiedDate
             , [CommaSeparated] List<string> sort
             , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetExchangeRateHeader.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetExchangeRateHeader.RequestFilter
                {
                    ExchangeRateCode = ExchangeRateCode,
                    Description = Description,
                    CurrencyCode = CurrencyCode,
                    RateTypeMin = RateTypeMin,
                    RateTypeMax = RateTypeMax,
                    CalculationTypeMin = CalculationTypeMin,
                    CalculationTypeMax = CalculationTypeMax,
                    ExpiredPeriod = ExpiredPeriod,
                    StatusMin = StatusMin,
                    StatusMax = StatusMax,
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

        [HttpGet]
        [Route("GetCurrentExchangeRate")]
        public async Task<IActionResult> GetCurrentExchangeRate(
              [CommaSeparated] string CurrencyCode
             , [CommaSeparated] DateTime TransactionDate
             , [CommaSeparated] int RateType)
        {
            var req = new GetCurrentExchangeRate.Request
            {
                Filter = new GetCurrentExchangeRate.RequestFilter
                {
                    CurrencyCode = CurrencyCode,
                    TransactionDate = TransactionDate,
                    RateType = RateType
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("ExchangeRate")]
        public async Task<IActionResult> DeleteNumberFormatHeader([FromBody] DeleteExchangeRate.RequestBodyDeleteExchangeRate body)
        {
            var req = new DeleteExchangeRate.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

    }
}
