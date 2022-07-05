using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.Port
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class PortController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PortController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Port")]
        public async Task<IActionResult> GetPort([CommaSeparated] List<string> PortCode
            , [CommaSeparated] List<string> PortName
            , [CommaSeparated] List<string> PortType
            , [CommaSeparated] List<string> CityName
            , [CommaSeparated] bool? InActive
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> ModifiedBy
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetPort.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetPort.RequestFilter
                {
                    PortCode = PortCode,
                    PortName = PortName,
                    PortType = PortType,
                    CityName = CityName,
                    InActive = InActive,
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
        [Route("Port")]
        public async Task<IActionResult> CreatePort([FromBody] PostPort.RequestPortBody body)
        {
            var req = new PostPort.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("Port")]
        public async Task<IActionResult> UpdatePort([FromBody] PutPort.RequestPortUpdate body)
        {
            var req = new PutPort.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("Port")]
        public async Task<IActionResult> DeletePort(DeletePort.RequestPortDelete body)
        {
            var req = new DeletePort.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
