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

namespace FLOG_BE.Features.Companies.Bank
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BankController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        [Route("Bank")]
        public async Task<IActionResult> CreateBank([FromBody] PostBank.RequestBankBody body)
        {
            var req = new PostBank.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("Bank")]
        public async Task<IActionResult> GetBank([CommaSeparated] List<string> BankCode
            , [CommaSeparated] List<string> BankName
            , [CommaSeparated] List<string> Address
            , [CommaSeparated] List<string> CityCode
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
            var req = new GetBank.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetBank.RequestFilter
                {
                    BankCode = BankCode,
                    BankName = BankName,
                    Address = Address,
                    CityName = CityName,
                    CityCode = CityCode,
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

        [HttpPut]
        [Route("Bank")]
        public async Task<IActionResult> UpdateBank([FromBody] PutBank.RequestBankUpdate body)
        {
            var req = new PutBank.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("Bank")]
        public async Task<IActionResult> DeleteBank(DeleteBank.RequestBankDelete body)
        {
            var req = new DeleteBank.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
