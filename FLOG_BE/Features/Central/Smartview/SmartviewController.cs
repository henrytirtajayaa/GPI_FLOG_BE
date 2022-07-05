using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Central.Smartview
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class SmartviewController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SmartviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetSmartView")]
        public async Task<IActionResult> GetSmartView(
        [CommaSeparated] List<string> GroupName
            , [CommaSeparated] List<string> SmartTitle
            , [CommaSeparated] List<string> SqlViewName
            , [CommaSeparated] bool? isFunction
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetSmartview.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetSmartview.RequestFilter
                {
                    GroupName = GroupName,
                    SmartTitle = SmartTitle,
                    SqlViewName = SqlViewName,
                    isFunction = isFunction
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("PostSmartView")]
        public async Task<IActionResult> CreateSmartView([FromBody] PostSmartview.RequestBodyPostSmartView body)
        {
            var req = new PostSmartview.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutSmartView")]
        public async Task<IActionResult> PutSmartView([FromBody] PutrSmartview.RequestBodyUpdateSmartview body)
        {
            var req = new PutrSmartview.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("DeleteSmartView")]
        public async Task<IActionResult> DeleteSmartView(DeleteSmartview.RequestBodyDeleteSmartview body)
        {
            var req = new DeleteSmartview.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetSmartViewByRoleId")]
        public async Task<IActionResult> GetSmartViewByRoleId(
            [CommaSeparated] Guid SecurityRoleId)
        {
            var req = new GetSmartviewByRoleId.Request
            {
                SecurityRoleId = SecurityRoleId
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetDefaultSmartview")]
        public async Task<IActionResult> GetDefaultSmartview(
            [CommaSeparated] Guid SmartviewId)
        {
            var req = new GetDefaultSmartview.Request
            {
                SmartviewId = SmartviewId
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("GetResultSmartview")]
        public async Task<IActionResult> GetResultSmartview(
            [FromBody] GetResultSmartview.Request request)
        {
            var req = new GetResultSmartview.Request
            {
                SmartviewId = request.SmartviewId,
                Filter = request.Filter
            };
            return ToActionResult(await _mediator.Send(req));
        }

    }
}
