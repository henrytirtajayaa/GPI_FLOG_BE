using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.CustomerVendorRelation
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class CustomerVendorRelationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerVendorRelationController(IMediator mediator)
        {
            _mediator = mediator;
        }

       
        [HttpPost]
        [Route("CustomerVendorRelation")]
        public async Task<IActionResult> CreateCustomerVendorRelation([FromBody] PostCustomerVendorRelation.RequestVendorBody body)
        {
           var req = new PostCustomerVendorRelation.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("CustomerVendorRelation")]
        public async Task<IActionResult> GetCustomerVendorRelation([CommaSeparated] List<string> CustomerCode
          , [CommaSeparated] List<string> CustomerName
          , [CommaSeparated] List<string> VendorCode
          , [CommaSeparated] List<string> VendorName
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
            var req = new GetCustomerVendorRelation.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetCustomerVendorRelation.RequestFilter
                {
                    CustomerCode = CustomerCode,
                    CustomerName = CustomerName,
                    VendorCode = VendorCode,
                    VendorName = VendorName,
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

        [HttpPut]
        [Route("CustomerVendorRelation")]
        public async Task<IActionResult> UpdateCustomerVendorRelation([FromBody] PutCustomerVendorRelation.RequestPutBody body)
        {
            var req = new PutCustomerVendorRelation.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("CustomerVendorRelation")]
        public async Task<IActionResult> DeleteCustomerVendorRelation(DeleteCustomerVendorRelation.RequestDelete body)
        {
            var req = new DeleteCustomerVendorRelation.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
