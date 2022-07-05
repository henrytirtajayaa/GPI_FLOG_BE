using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using System;

namespace FLOG_BE.Features.Companies.ContainerDepot
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ContainerDepotController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContainerDepotController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        [Route("ContainerDepot")]
        public async Task<IActionResult> CreateContainerDepot([FromBody] PostContainerDepot.RequestContainerDepotBody body)
        {
            var req = new PostContainerDepot.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("ContainerDepot")]
        public async Task<IActionResult> GetContainerDepot([CommaSeparated] List<string> DepotCode
            , [CommaSeparated] List<string> DepotName
            , [CommaSeparated] List<string> VendorName
            , [CommaSeparated] List<string> CityName
            , [CommaSeparated] bool? InActive
            , [CommaSeparated] List<string> ModifiedBy
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetContainerDepot.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetContainerDepot.RequestFilter
                {
                    DepotCode = DepotCode,
                    DepotName = DepotName,
                    VendorName = VendorName,
                    CityName = CityName,
                    InActive = InActive,
                    ModifiedBy = ModifiedBy,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("ContainerDepot")]
        public async Task<IActionResult> UpdateContainerDepot([FromBody] PutContainerDepot.RequestContainerDepotUpdate body)
        {
            var req = new PutContainerDepot.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("ContainerDepot")]
        public async Task<IActionResult> DeleteContainerDepot(DeleteContainerDepot.RequestContainerDepotDelete body)
        {
            var req = new DeleteContainerDepot.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
