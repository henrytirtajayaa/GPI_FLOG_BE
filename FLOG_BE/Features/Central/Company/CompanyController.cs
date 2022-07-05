using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Central.Company
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Company")]
        public async Task<IActionResult> GetCompany([CommaSeparated] List<string> CompanyName
            , [CommaSeparated] List<string> DatabaseId
            , [CommaSeparated] List<string> DatabaseAddress
            , [CommaSeparated] List<string> CoaSymbol
            , [CommaSeparated] List<int> CoaTotalLengthMin
            , [CommaSeparated] List<int> CoaTotalLengthMax
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> ModifiedBy
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd
            , [CommaSeparated] bool? InActive
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
             var req = new GetCompany.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetCompany.RequestFilter
                {
                    CompanyName = CompanyName,
                    DatabaseId = DatabaseId,
                    DatabaseAddress = DatabaseAddress,
                    CoaSymbol = CoaSymbol,
                    CoaTotalLengthMin = CoaTotalLengthMin,
                    CoaTotalLengthMax = CoaTotalLengthMax,
                    CreatedBy = CreatedBy,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    InActive = InActive
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("Company")]
        public async Task<IActionResult> UpdateCompany([FromBody] PutCompany.RequestBodyUpdate body)
        {
            var req = new PutCompany.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("Company")]
        public async Task<IActionResult> CreateCompany([FromBody] PostCompany.RequestBody body)
        {
            var req = new PostCompany.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("Company")]
        public async Task<IActionResult> DeleteCompany(DeleteCompany.RequestBodyDelete body)
        {
            var req = new DeleteCompany.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

     

    }
}
