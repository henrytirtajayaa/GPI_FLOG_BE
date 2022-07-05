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

namespace FLOG_BE.Features.Companies.ReceivableSetup
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ReceivableSetupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReceivableSetupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("ReceivableSetup")]
        public async Task<IActionResult> GetReceivableSetup(
            [CommaSeparated] List<int> DefaultRateType
            , [CommaSeparated] List<string> TransactionType
            , [CommaSeparated] List<int> TaxRateType
            ,[CommaSeparated] List<bool> AgingByDocdate
            ,[CommaSeparated] List<decimal> WriteoffLimit
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetReceivableSetup.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetReceivableSetup.RequestFilter
                {
                    TransactionType = TransactionType,
                    DefaultRateType = DefaultRateType,
                    TaxRateType = TaxRateType,
                    AgingByDocdate = AgingByDocdate,
                    WriteoffLimit = WriteoffLimit
                }
            };

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("ReceivableSetup")]
        public async Task<IActionResult> CreateReceivableSetup([FromBody] PostReceivableSetup.RequestBodyPostReceivableSetup body)
        {
            var req = new PostReceivableSetup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("ReceivableSetup")]
        public async Task<IActionResult> UpdateReceivableSetup([FromBody] PutReceivableSetup.RequestBodyUpdateReceivableSetup body)
        {
            var req = new PutReceivableSetup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("ReceivableSetup")]
        public async Task<IActionResult> DeleteReceivableSetup([FromBody] DeleteReceivableSetup.RequestBodyDelete body)
        {
            var req = new DeleteReceivableSetup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

    }
}
