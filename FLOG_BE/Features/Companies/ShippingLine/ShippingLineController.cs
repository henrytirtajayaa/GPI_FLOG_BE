using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.ShippingLine
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class ShippingLineController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShippingLineController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("ShippingLine")]
        public async Task<IActionResult> GetShippingLine(
             [CommaSeparated] List<string> ShippingLineCode
            , [CommaSeparated] List<string> ShippingLineName
            , [CommaSeparated] List<string> ShippingLineType
            , [CommaSeparated] List<Guid?> VendorId
            , [CommaSeparated] bool? IsFeeder
            , [CommaSeparated] bool? Inactive
            , [CommaSeparated] List<int?> Status
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
            var req = new GetShippingLine.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetShippingLine.RequestFilter
                {
                    ShippingLineCode = ShippingLineCode,
                    ShippingLineName = ShippingLineName,
                    ShippingLineType = ShippingLineType,
                    VendorId = VendorId,
                    IsFeeder = IsFeeder,
                    Status = Status,
                    Inactive = Inactive,
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
        [Route("ShippingLine")]
        public async Task<IActionResult> CreateShippingLine([FromBody] PostShippingLine.RequestShippingLineBody body)
        {
            var req = new PostShippingLine.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("ShippingLine")]
        public async Task<IActionResult> UpdateShippingLine([FromBody] PutShippingLine.RequestPutShippingLineBody body)
        {
            var req = new PutShippingLine.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
        
        [HttpDelete]
        [Route("ShippingLine")]
        public async Task<IActionResult> DeleteShippingLine(DeleteShippingLine.RequestShippingLineDelete body)
        {
            var req = new DeleteShippingLine.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
