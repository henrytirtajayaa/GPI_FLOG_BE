using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Infrastructure.Attributes.CommaSeparated;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FLOG_BE.Features.Companies.PayableSetup
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PayableSetupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PayableSetupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("PayableSetup")]
        public async Task<IActionResult> GetPayableSetup(
        [CommaSeparated] List<int> DefaultRateType
        , [CommaSeparated] List<int> TaxRateType
        , [CommaSeparated] List<bool> AgingByDocdate
        , [CommaSeparated] List<decimal> WriteoffLimit
        , [CommaSeparated] List<string> sort
        , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetPayableSetup.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetPayableSetup.RequestFilter
                {
                    DefaultRateType = DefaultRateType,
                    TaxRateType = TaxRateType,
                    AgingByDocdate = AgingByDocdate,
                    WriteoffLimit = WriteoffLimit
                }
            };

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("PayableSetup")]
        public async Task<IActionResult> CreatePayableSetup([FromBody] PostPayableSetup.RequestBodyPostPayableSetup body)
        {
            var req = new PostPayableSetup.Request
            {
                Body = body
            };

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PayableSetup")]
        public async Task<IActionResult> UpdatePayableSetup([FromBody] PutPayableSetup.RequestBodyUpdatePayableSetup body)
        {
            var req = new PutPayableSetup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
