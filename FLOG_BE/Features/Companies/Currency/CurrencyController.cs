using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.Currency
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class CurrencyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CurrencyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Currency")]
        public async Task<IActionResult> GetAccount([CommaSeparated] List<string> CurrencyCode
            , [CommaSeparated] List<string> Description
            , [CommaSeparated] List<string> Symbol
            , [CommaSeparated] List<int?> DecimalPlacesMin
            , [CommaSeparated] List<int?> DecimalPlacesMax
            , [CommaSeparated] List<string> RealizedGainAcc
            , [CommaSeparated] List<string> RealizedLossAcc
            , [CommaSeparated] List<string> UnrealizedGainAcc
            , [CommaSeparated] List<string> UnrealizedLossAcc
            , [CommaSeparated] bool? Inactive
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> ModifiedBy
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd
            , [CommaSeparated] List<string> CurrencyUnit
            , [CommaSeparated] List<string> CurrencySubUnit
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetCurrency.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetCurrency.RequestFilter
                {
                    CurrencyCode = CurrencyCode,
                    Description = Description,
                    Symbol = Symbol,
                    DecimalPlacesMin = DecimalPlacesMin,
                    DecimalPlacesMax = DecimalPlacesMax,
                    RealizedGainAcc = RealizedGainAcc,
                    RealizedLossAcc = RealizedLossAcc,
                    UnrealizedGainAcc = UnrealizedGainAcc,
                    UnrealizedLossAcc = UnrealizedLossAcc,
                    Inactive = Inactive,
                    CreatedBy = CreatedBy,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    CurrencyUnit = CurrencyUnit,
                    CurrencySubUnit = CurrencySubUnit
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("Currency")]
        public async Task<IActionResult> CreateAccount([FromBody] PostCurrency.RequestCurrencyBody body)
        {
            var req = new PostCurrency.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("Currency")]
        public async Task<IActionResult> UpdateAccount([FromBody] PutCurrency.RequestPutCurrencyBody body)
        {
            var req = new PutCurrency.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("Currency")]
        public async Task<IActionResult> DeleteAccount(DeleteCurrency.RequestCurrencyDelete body)
        {
            var req = new DeleteCurrency.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }


    }
}
