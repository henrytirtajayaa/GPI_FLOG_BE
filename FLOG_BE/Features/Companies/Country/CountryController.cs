using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.Country
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class CountryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CountryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Country")]
        public async Task<IActionResult> GetCountry(
            [CommaSeparated] List<string> CountryCode
            , [CommaSeparated] List<string> CountryName
            , [CommaSeparated] bool? InActive
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> ModifiedBy
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100
            , [FromQuery] int offset = 0)
        {
            var req = new GetCountry.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetCountry.RequestFilter
                {
                    CountryCode = CountryCode,
                    CountryName = CountryName,
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
        [Route("Country")]
        public async Task<IActionResult> CreateCountry([FromBody] PostCountry.RequestBodyPostCountry body)
        {
            var req = new PostCountry.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("Country")]
        public async Task<IActionResult> UpdateCountry([FromBody] PutCountry.RequestBodyUpdateCountry body)
        {
            var req = new PutCountry.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("Country")]
        public async Task<IActionResult> DeleteCountry(DeleteCountry.RequestBodyDeleteCountry body)
        {
            var req = new DeleteCountry.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
