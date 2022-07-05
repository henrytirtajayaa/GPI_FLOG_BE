using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.VehicleType
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class VehicleTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VehicleTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("VehicleType")]
        public async Task<IActionResult> GetVehicleType([CommaSeparated] List<string> VehicleTypeCode
            , [CommaSeparated] List<string> VehicleTypeName
            , [CommaSeparated] List<string> VehicleCategory
            , [CommaSeparated] bool? Inactive
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
             var req = new GetVehicleType.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetVehicleType.RequestFilter
                {
                    VehicleTypeCode = VehicleTypeCode,
                    VehicleTypeName = VehicleTypeName,
                    VehicleCategory = VehicleCategory,
                    Inactive = Inactive
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("VehicleType")]
        public async Task<IActionResult> UpdateVehicleType([FromBody] PutVehicleType.RequestBodyUpdateVehicleType body)
        {
            var req = new PutVehicleType.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("VehicleType")]
        public async Task<IActionResult> CreateVehicleType([FromBody] PostVehicleType.RequestBody body)
        {
            var req = new PostVehicleType.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("VehicleType")]
        public async Task<IActionResult> DeleteVehicleType(DeleteVehicleType.RequestBodyVehicleDelete body)
        {
            var req = new DeleteVehicleType.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

     

    }
}
