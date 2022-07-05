using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.ApprovalSetup
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
        [Route("ApprovalSetup")]
        public async Task<IActionResult> GetApprovalSetup(
          [CommaSeparated] List<string> ApprovalCode
          , [CommaSeparated] List<string> Description
          , [CommaSeparated] int? Status
          , [CommaSeparated] List<string> CreatedBy
          , [CommaSeparated] List<DateTime?> CreatedDateStart
          , [CommaSeparated] List<DateTime?> CreatedDateEnd
          , [CommaSeparated] List<string> ModifiedBy
          , [CommaSeparated] List<DateTime?> ModifiedDateStart
          , [CommaSeparated] List<DateTime?> ModifiedDateEnd
          , [CommaSeparated] List<string> sort
          , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetApprovalSetup.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetApprovalSetup.RequestFilter
                {
                    ApprovalCode = ApprovalCode,
                    Description = Description,
                    Status = Status,
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
        [Route("ApprovalSetupWithPerson")]
        public async Task<IActionResult> ApprovalSetupWithPerson(
          [CommaSeparated] List<string> ApprovalCode
          , [CommaSeparated] List<string> Description
          , [CommaSeparated] int? Status
          , [CommaSeparated] List<string> CreatedBy
          , [CommaSeparated] List<DateTime?> CreatedDateStart
          , [CommaSeparated] List<DateTime?> CreatedDateEnd
          , [CommaSeparated] List<string> ModifiedBy
          , [CommaSeparated] List<DateTime?> ModifiedDateStart
          , [CommaSeparated] List<DateTime?> ModifiedDateEnd
          , [CommaSeparated] List<string> sort
          , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetApprovalSetupWithPerson.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetApprovalSetupWithPerson.RequestFilter
                {
                    ApprovalCode = ApprovalCode,
                    Description = Description,
                    Status = Status,
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
        [Route("ApprovalSetup")]
        public async Task<IActionResult> CreateApprovalSetup([FromBody] PostApprovalSetup.RequestApprovalSetupHeader body)
        {
            var req = new PostApprovalSetup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("ApprovalSetup")]
        public async Task<IActionResult> UpdateApprovalSetup([FromBody] PutApprovalSetup.RequestApprovalSetupHeader body)
        {
            var req = new PutApprovalSetup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("ApprovalSetup")]
        public async Task<IActionResult> DeleteApprovalSetup([FromBody] DeleteApprovalSetup.RequestBodyApprovalSetup body)
        {
            var req = new DeleteApprovalSetup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
