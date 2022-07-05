using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.ApprovalSetupDetail
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
        [Route("ApprovalSetupDetail")]
        public async Task<IActionResult> GetApprovalSetupDetail([CommaSeparated] List<string> Description
              , [CommaSeparated] List<int> Level
              , [CommaSeparated] bool? HasLimit
              , [CommaSeparated] List<decimal?> ApprovalLimit
              , [CommaSeparated] List<int?> Status
              , [CommaSeparated] List<string> sort
              , [FromQuery] int limit = 100, [FromQuery] int offset = 0, [FromQuery] string ApprovalSetupHeaderId = "")
        {
            var req = new GetApprovalSetupDetail.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetApprovalSetupDetail.RequestFilter
                {
                    Description = Description,
                    Level = Level,
                    HasLimit = HasLimit,
                    ApprovalLimit = ApprovalLimit,
                    Status = Status,
                    ApprovalSetupHeaderId = (ApprovalSetupHeaderId != null ? Guid.Parse(ApprovalSetupHeaderId) : Guid.Empty)
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }


      
    }
}
