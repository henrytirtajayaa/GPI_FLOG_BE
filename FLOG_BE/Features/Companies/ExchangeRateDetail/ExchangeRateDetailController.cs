using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.ExchangeRateDetail
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ExchangeRateDetailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExchangeRateDetailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("ExchangeRateDetail")]
        public async Task<IActionResult> GetExchangeRateDetail([CommaSeparated] List<DateTime?> RateDateStart
              , [CommaSeparated] List<DateTime?> RateDateEnd
              , [CommaSeparated] List<DateTime?> ExpiredDateStart
              , [CommaSeparated] List<DateTime?> ExpiredDateEnd
              , [CommaSeparated] List<decimal?> RateAmountMin
              , [CommaSeparated] List<decimal?> RateAmountMax
              , [CommaSeparated] List<int?> StatusMin
              , [CommaSeparated] List<int?> StatusMax
              , [CommaSeparated] List<string> sort
              , [FromQuery] int limit = 100, [FromQuery] int offset = 0, [FromQuery] string ExchangeRateHeaderId = "")
        {
            var req = new GetExchangeRateDetail.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetExchangeRateDetail.RequestFilter
                {
                    RateDateStart = RateDateStart,
                    RateDateEnd = RateDateEnd,
                    ExpiredDateStart = ExpiredDateStart,
                    ExpiredDateEnd = ExpiredDateEnd,
                    RateAmountMin = RateAmountMin,
                    RateAmountMax = RateAmountMax,
                    StatusMin = StatusMin,
                    StatusMax = StatusMax,
                    ExchangeRateHeaderId = (ExchangeRateHeaderId != null ? Guid.Parse(ExchangeRateHeaderId) : Guid.Empty)
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }



    }
}
