using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Finance.GLClosing
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class GLClosingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GLClosingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetUnclosingPeriod")]
        public async Task<IActionResult> GetUnclosingPeriod()
        {
            var req = new GetUnClosingPeriod.Request();

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetClosingPeriod")]
        public async Task<IActionResult> GetClosingPeriod()
        {
            var req = new GetClosingPeriod.Request();

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("GLClosingMonth")]
        public async Task<IActionResult> PostGLClosingMonth([FromBody] PostGLClosingMonth.RequestFormBody body)
        {
            var req = new PostGLClosingMonth.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("GLUnclosingMonth")]
        public async Task<IActionResult> PostGLUnclosingMonth([FromBody] PostGLUnclosingMonth.RequestFormBody body)
        {
            var req = new PostGLUnclosingMonth.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

    }
}
