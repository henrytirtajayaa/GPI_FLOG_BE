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

namespace FLOG_BE.Features.Companies.MSDepartment
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class MsDepartmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MsDepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("MsDepartment")]
        public async Task<IActionResult> GetTransactionType(
        [CommaSeparated] List<string> DepartmentCode
        , [CommaSeparated] List<string> DepartmentName
        , [CommaSeparated] List<string> sort
        , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetMsDepartment.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetMsDepartment.RequestFilter
                {
                    DepartmentCode = DepartmentCode,
                    DepartmentName = DepartmentName
                }
            };

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("MsDepartment")]
        public async Task<IActionResult> CreateTransactionType([FromBody] PostMsDepartment.RequestPostMsDepartment body)
        {
            var req = new PostMsDepartment.Request
            {
                Body = body
            };

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("MsDepartment")]
        public async Task<IActionResult> UpdateTransactionType([FromBody] PutMsDepartment.RequestUpdateDepartment body)
        {
            var req = new PutMsDepartment.Request
            {
                Body = body
            };

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("MsDepartment")]
        public async Task<IActionResult> DeleteTransactionType([FromBody] DeleteMsDepartment.RequestDeleteMsDepartment body)
        {
            var req = new DeleteMsDepartment.Request
            {
                Body = body
            };

            return ToActionResult(await _mediator.Send(req));
        }
    }
}
