using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.City
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class CityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CityController(IMediator mediator)
        {
            _mediator = mediator;
        }
 
        [HttpGet]
        [Route("City")]
        public async Task<IActionResult> GetCity([CommaSeparated] List<string> CityCode
           , [CommaSeparated] List<string> CityName
           , [CommaSeparated] List<string> Province
           , [CommaSeparated] List<string> CountryName
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
            var req = new GetCity.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetCity.RequestFilter
                {
                    CityCode = CityCode,
                    CityName = CityName,
                    Province = Province,
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


        [HttpPut]
        [Route("City")]
        public async Task<IActionResult> UpdateBank([FromBody] PutCity.RequestCityUpdate body)
        {
            var req = new PutCity.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }


        [HttpPost]
        [Route("City")]
        public async Task<IActionResult> CreateBank([FromBody] PostCity.RequestCityBody body)
        {
            var req = new PostCity.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("City")]
        public async Task<IActionResult> DeleteCity(DeleteCity.RequestCityDelete body)
        {
            var req = new DeleteCity.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
