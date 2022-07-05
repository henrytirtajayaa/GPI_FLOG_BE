using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using Infrastructure.Attributes.CommaSeparated;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FLOG_BE.Features.Report.Dashboard
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("DashboardMyTasks")]
        public async Task<IActionResult> GetDashboardTasks()
        {
            var req = new GetMyTask.Request();

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("DashboardMyApprovals")]
        public async Task<IActionResult> DashboardMyApprovals()
        {
            var req = new GetMyApprovalList.Request();

            return ToActionResult(await _mediator.Send(req));
        }

    }
}
