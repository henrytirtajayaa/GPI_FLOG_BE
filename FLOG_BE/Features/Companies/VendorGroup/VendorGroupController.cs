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

namespace FLOG_BE.Features.Companies.VendorGroup
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class VendorGroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VendorGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("VendorGroup")]
        public async Task<IActionResult> GetVendorGroup(
            [CommaSeparated] List<string> VendorGroupCode
            , [CommaSeparated] List<string> VendorGroupName
            , [CommaSeparated] List<string> PaymentTermCode
            , [CommaSeparated] List<string> PayableAccountNo
            , [CommaSeparated] List<string> AccruedPayableAccountNo
            , [CommaSeparated] bool? InActive
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
            var req = new GetVendorGroup.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetVendorGroup.RequestFilter
                {
                    VendorGroupCode = VendorGroupCode,
                    VendorGroupName = VendorGroupName,
                    PaymentTermCode = PaymentTermCode,
                    PayableAccountNo = PayableAccountNo,
                    AccruedPayableAccountNo = AccruedPayableAccountNo,
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
        [Route("VendorGroup")]
        public async Task<IActionResult> CreateVendorGroup([FromBody] PostVendorGroup.RequestBodyPostVendorGroup body)
        {
            var req = new PostVendorGroup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("VendorGroup")]
        public async Task<IActionResult> UpdateVendorGroup([FromBody] PutVendorGroup.RequestBodyUpdateVendorGroup body)
        {
            var req = new PutVendorGroup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("VendorGroup")]
        public async Task<IActionResult> DeleteVendorGroup([FromBody] DeleteVendorGroup.RequestBodyDeleteVendorGroup body)
        {
            var req = new DeleteVendorGroup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
