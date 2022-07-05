using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.ChargeGroup
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class ChargeGroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChargeGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("ChargeGroup")]
        public async Task<IActionResult> ChargeGroup([CommaSeparated] List<string> ChargeGroupCode
            , [CommaSeparated] List<string> ChargeGroupName
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetChargeGroup.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetChargeGroup.RequestFilter
                {
                    ChargeGroupCode = ChargeGroupCode,
                    ChargeGroupName = ChargeGroupName,
                    
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("ChargeGroup")]
        public async Task<IActionResult> UpdateChargeGroup([FromBody] PutChargeGroup.RequestPutChargeBody body)
        {
            var req = new PutChargeGroup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("ChargeGroup")]
        public async Task<IActionResult> PostChargeGroup([FromBody] PostChargeGroup.RequestChargeGroupBody body)
        {
            var req = new PostChargeGroup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("ChargeGroup")]
        public async Task<IActionResult> DeleteCharges(DeleteChargeGroup.RequestChargeGroupDelete body)
        {
            var req = new DeleteChargeGroup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
