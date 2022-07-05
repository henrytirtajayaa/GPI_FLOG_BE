using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FLOG_BE.Features.Central.SecurityRoles.Forms
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
        [Route("GetFormGroup")]
        public async Task<IActionResult> GetFormGroup()
        {
            var req = new GetFormGroup.Request { };
            return ToActionResult(await _mediator.Send(req));
        }


        [HttpGet]
        [Route("GetForm")]
        public async Task<IActionResult> GetForm([CommaSeparated] List<string> Module
            ,[CommaSeparated] List<string> FormName
            ,[CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new SecurityRoles.Forms.GetForm.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetForm.RequestFilterForm
                {
                    Module = Module,
                    FormName = FormName

                }
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
