using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Central.SecurityRoles
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class FormRoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FormRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("SecurityRole")]
        public async Task<IActionResult> SecurityRole([CommaSeparated] List<string> RoleName
            ,[CommaSeparated] List<string> RoleCode
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
            var req = new GetSecurityRole.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetSecurityRole.RequestFilter
                {
                    RoleCode = RoleCode,
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
        [HttpGet]
        [Route("SecurityRoleDetail")]
        public async Task<IActionResult> GetSecurityRoleDetail([CommaSeparated] List<string> RoleId
          , [CommaSeparated] List<string> sort
          , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetSecurityRoleDetail.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetSecurityRoleDetail.RequestFilter
                {
                    RoleId = RoleId

                }
            };
            return ToActionResult(await _mediator.Send(req));
        }
        [HttpGet]
        [Route("SecuritySmartRole")]
        public async Task<IActionResult> GetSecuritySmartRole([CommaSeparated] string SecurityRoleId
          , [CommaSeparated] List<string> sort
          , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetSecuritySmartView.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetSecuritySmartView.RequestFilter
                {
                    SecurityRoleId = SecurityRoleId

                }
            };
            return ToActionResult(await _mediator.Send(req));
        }
        [HttpPost]
        [Route("SecurityRole")]
        public async Task<IActionResult> CreateSecurityRole([FromBody] PostSecurityRole.RequestRole body)
        {
            var req = new PostSecurityRole.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("SecurityRole")]
        public async Task<IActionResult> UpdateSecurityRole([FromBody] PutSecurityRole.RequestBodyRoleUpdate body)
        {
            var req = new PutSecurityRole.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("SecurityRole")]
        public async Task<IActionResult> DeleteCompany(DeleteSecurityRole.RequestBodySecurityRoleDelete body)
        {
            var req = new DeleteSecurityRole.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
        

    }
}
