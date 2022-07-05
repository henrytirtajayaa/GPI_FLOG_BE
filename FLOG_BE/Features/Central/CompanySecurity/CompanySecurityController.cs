using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Central.CompanySecurity
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class CompanySecurityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanySecurityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("CompanySecurity")]
        public async Task<IActionResult> GetCompanySecurity([CommaSeparated] List<string> UserName
            , [CommaSeparated] List<string> CompanyName
            , [CommaSeparated] List<string> RoleName
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
            var req = new GetCompanySecurity.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetCompanySecurity.RequestFilter
                {
                    CompanyName = CompanyName,
                    UserName = UserName,
                    RoleName = RoleName,
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
        [Route("CompanySecurity")]
        public async Task<IActionResult> PutCompany([FromBody] PutCompanySecurity.RequestUpdateCS body)
        {
            var req = new PutCompanySecurity.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("CompanySecurity")]
        public async Task<IActionResult> CreateCompany([FromBody] PostCompanySecurity.RequestBodyCS body)
        {
            var req = new PostCompanySecurity.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("CompanySecurity")]
        public async Task<IActionResult> DeleteUserGroup(DeleteCompanySecurity.RequestBodyDeleteCompanySecurity body)
        {
            var req = new DeleteCompanySecurity.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}