using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.CompanyBranch
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class CompanyBranchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyBranchController(IMediator mediator)
        {
            _mediator = mediator;
        }
 
        [HttpGet]
        [Route("CompanyBranch")]
        public async Task<IActionResult> GetCompanyBranch([CommaSeparated] List<string> BranchCode
           , [CommaSeparated] List<string> BranchName
           , [CommaSeparated] bool? Default
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
            var req = new GetCompanyBranch.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetCompanyBranch.RequestFilter
                {
                    BranchCode = BranchCode,
                    BranchName = BranchName,
                    Default = Default,
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

        [HttpPut]
        [Route("CompanyBranch")]
        public async Task<IActionResult> UpdateCompanyBranch([FromBody] PutCompanyBranch.RequestPutUpdate body)
        {
            var req = new PutCompanyBranch.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }


        [HttpPost]
        [Route("CompanyBranch")]
        public async Task<IActionResult> CreateCompanyBranch([FromBody] PostCompanyBranch.RequestPostBody body)
        {
            var req = new PostCompanyBranch.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("CompanyBranch")]
        public async Task<IActionResult> DeleteCompanyBranch(DeleteCompanyBranch.RequestDelete body)
        {
            var req = new DeleteCompanyBranch.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
