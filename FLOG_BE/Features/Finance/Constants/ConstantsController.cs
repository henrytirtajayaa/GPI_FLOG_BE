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

namespace FLOG_BE.Features.Finance.Constants
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class ConstantsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConstantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("DocStatus")]
        public async Task<IActionResult> GetDocStatus()
        {
            var req = new GetDocStatus.Request();

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("CalcMethod")]
        public async Task<IActionResult> GetCalcMethod()
        {
            var req = new GetCalcMethod.Request();

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("RateType")]
        public async Task<IActionResult> GetRateType()
        {
            var req = new GetRateType.Request();

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("MinInputDate")]
        public async Task<IActionResult> GetMinInputDate(int TrxModule)
        {
            var req = new GetMinInputDate.Request
            {
                Filter = new GetMinInputDate.RequestFilter
                {
                    TrxModule = TrxModule
                }
            };

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("PostingParam")]
        public async Task<IActionResult> GetPostingParam()
        {
            var req = new GetPostingParam.Request();

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PostingParam")]
        public async Task<IActionResult> UpdatePostingParam([FromBody] PutPostingParam.Request body)
        {
            var req = new PutPostingParam.Request
            {
                PostingParams = body.PostingParams
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("TrxModule")]
        public async Task<IActionResult> GetTrxModule()
        {
            var req = new GetTrxModule.Request();

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("DocNumberSetup")]
        public async Task<IActionResult> GetDocNumberSetup()
        {
            var req = new GetDocNoSetup.Request();

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("DocNumberSetup")]
        public async Task<IActionResult> UpdateDocNumberSetup([FromBody] PutDocNoSetup.Request body)
        {
            var req = new PutDocNoSetup.Request
            {
                DocNumberSetups = body.DocNumberSetups,
                TrxTypeSetups = body.TrxTypeSetups
            };
            return ToActionResult(await _mediator.Send(req));
        }

    }
}
