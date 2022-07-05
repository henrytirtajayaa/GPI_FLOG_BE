using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.SalesPerson
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class SalesPersonController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalesPersonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("SalesPerson")]
        public async Task<IActionResult> GetCharges([CommaSeparated] List<string> SalesCode
            , [CommaSeparated] List<string> SalesName

            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetSalesPerson.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetSalesPerson.RequestFilter
                {
                    SalesCode = SalesCode,
                    SalesName = SalesName
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("SalesPerson")]
        public async Task<IActionResult> UpdateCharges([FromBody] PutSalesPerson.RequestPutSalesPersonBody body)
        {
            var req = new PutSalesPerson.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("SalesPerson")]
        public async Task<IActionResult> PostCharges([FromBody] PostSalesPerson.RequestSalesPersonBody body)
        {
            var req = new PostSalesPerson.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("SalesPerson")]
        public async Task<IActionResult> DeleteCharges(DeleteSalesPerson.RequestSalesPersonDelete body)
        {
            var req = new DeleteSalesPerson.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
