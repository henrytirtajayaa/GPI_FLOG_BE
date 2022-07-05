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

namespace FLOG_BE.Features.Companies.FinancialSetup
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class FinancialSetupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FinancialSetupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("FinancialSetup")]
        public async Task<IActionResult> GetFinancialSetup(
            [CommaSeparated] List<string> FuncCurrencyCode
            , [CommaSeparated] List<int> DefaultRateType
            , [CommaSeparated] List<int> TaxRateType
            , [CommaSeparated] List<string> UomScheduleCode
            , [CommaSeparated] List<int> DeptSegmentNo
            , [CommaSeparated] List<string> CheckbookChargesType
            , [CommaSeparated] List<int> Status
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetFinancialSetup.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetFinancialSetup.RequestFilter
                {
                   FuncCurrencyCode = FuncCurrencyCode,
                   DefaultRateType = DefaultRateType,
                   TaxRateType = TaxRateType,
                   UomScheduleCode = UomScheduleCode,
                   DeptSegmentNo = DeptSegmentNo,
                   CheckbookChargesType = CheckbookChargesType,
                   Status = Status
                }
            };

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("FinancialSetup")]
        public async Task<IActionResult> CreateFinancialSetup([FromBody] PostFinancialSetup.RequestBodyPostFinancialSetup body)
        {
            var req = new PostFinancialSetup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("FinancialSetup")]
        public async Task<IActionResult> UpdateFinancialSetup([FromBody] PutFinancialSetup.RequestBodyUpdateFinancialSetup body)
        {
            var req = new PutFinancialSetup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

    }
}
