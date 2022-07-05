using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.CustomerGroup
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class CustomerGroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("CustomerGroup")]
        public async Task<IActionResult> GetCustomerGroup(
            [CommaSeparated] List<string> CustomerGroupCode
            , [CommaSeparated] List<string> CustomerGroupName
            , [CommaSeparated] List<string> PaymentTermCode
            , [CommaSeparated] List<string> ReceivableAccountNo
            , [CommaSeparated] List<string> AccruedReceivableAccountNo
            , [CommaSeparated] bool? Inactive
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> ModifiedBy
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetCustomerGroup.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetCustomerGroup.RequestFilter
                {
                    CustomerGroupCode = CustomerGroupCode,
                    CustomerGroupName = CustomerGroupName,
                    PaymentTermCode = PaymentTermCode,
                    ReceivableAccountNo = ReceivableAccountNo,
                    AccruedReceivableAccountNo = AccruedReceivableAccountNo,
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
        [Route("CustomerGroup")]
        public async Task<IActionResult> CreateCustomerGroup([FromBody] PostCustomerGroup.RequestPostBody body)
        {
            var req = new PostCustomerGroup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        
        [HttpPut]
        [Route("CustomerGroup")]
        public async Task<IActionResult> UpdateCustomerGroup([FromBody] PutCustomerGroup.RequestPutBody body)
        {
            var req = new PutCustomerGroup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("CustomerGroup")]
        public async Task<IActionResult> DeleteCustomerGroup(DeleteCustomerGroup.RequestDeleteBody body)
        {
            var req = new DeleteCustomerGroup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }        
    }
}
