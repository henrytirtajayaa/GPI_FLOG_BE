using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.FiscalPeriodHeader
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class FiscalPeriodHeaderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FiscalPeriodHeaderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("FiscalPeriodHeader")]
       
        public async Task<IActionResult> GetFiscalPeriodHeader([CommaSeparated] List<int?> PeriodYearMin
            , [CommaSeparated] List<int?> PeriodYearMax
            , [CommaSeparated] List<int?> TotalPeriodMin
            , [CommaSeparated] List<int?> TotalPeriodMax
            , [CommaSeparated] List<DateTime?> StartDateStart
            , [CommaSeparated] List<DateTime?> StartDateEnd
            , [CommaSeparated] List<DateTime?> EndDateStart
            , [CommaSeparated] List<DateTime?> EndDateEnd
            , [CommaSeparated] bool? ClosingYear
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
            var req = new GetFiscalPeriodHeader.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetFiscalPeriodHeader.RequestFilter
                {
                    PeriodYearMin = PeriodYearMin,
                    PeriodYearMax = PeriodYearMax,
                    TotalPeriodMin = TotalPeriodMin,
                    TotalPeriodMax = TotalPeriodMax,
                    StartDateStart = StartDateStart,
                    StartDateEnd = StartDateEnd,
                    EndDateStart = EndDateStart,
                    EndDateEnd = EndDateEnd,
                    ClosingYear = ClosingYear,
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
        [Route("FiscalPeriodHeader")]
        public async Task<IActionResult> CreateFiscalPeriodHeader([FromBody] PostFiscalPeriodHeader.RequestFiscalBody body)
        {
            var req = new PostFiscalPeriodHeader.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("FiscalPeriodHeader")]
        public async Task<IActionResult> UpdateFiscalPeriodHeader([FromBody] PutFiscalPeriodHeader.RequestPutFiscalBody body)
        {
            var req = new PutFiscalPeriodHeader.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("FiscalPeriodHeader")]
        public async Task<IActionResult> DeleteFiscalPeriodHeader(DeleteFiscalPeriodHeader.RequestFiscalDelete body)
        {
            var req = new DeleteFiscalPeriodHeader.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

    }
}
