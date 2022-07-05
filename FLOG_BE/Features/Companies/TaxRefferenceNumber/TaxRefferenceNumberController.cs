using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using Infrastructure.Attributes.CommaSeparated;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace FLOG_BE.Features.Companies.TaxRefferenceNumber
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TaxRefferenceNumberController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TaxRefferenceNumberController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("TaxRefferenceNumber")]
        public async Task<IActionResult> GetTaxRefferenceNumber(
            [CommaSeparated] List<DateTime?> StartDateStart
            , [CommaSeparated] List<DateTime?> StartDateEnd
            , [CommaSeparated] List<int?> ReffNoStartMin
            , [CommaSeparated] List<int?> ReffNoStartMax
            , [CommaSeparated] List<int?> ReffNoEndMin
            , [CommaSeparated] List<int?> ReffNoEndMax
            , [CommaSeparated] List<int?> DocLengthMin
            , [CommaSeparated] List<int?> DocLengthMax
            , [CommaSeparated] List<int?> LastNoMin
            , [CommaSeparated] List<int?> LastNoMax
            , [CommaSeparated] List<int?> Status
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> ModifiedBy
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetTaxRefferenceNumber.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetTaxRefferenceNumber.RequestFilter
                {
                    StartDateStart = StartDateStart,
                    StartDateEnd = StartDateEnd,
                    ReffNoStartMin = ReffNoStartMin,
                    ReffNoStartMax = ReffNoStartMax,
                    ReffNoEndMin = ReffNoEndMin,
                    ReffNoEndMax = ReffNoEndMax,
                    DocLengthMin = DocLengthMin,
                    DocLengthMax = DocLengthMax,
                    LastNoMin = LastNoMin,
                    LastNoMax = LastNoMax,
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
        [Route("TaxRefferenceNumber")]
        public async Task<IActionResult> CreateTaxRefferenceNumber([FromBody] PostTaxRefferenceNumber.RequestTaxRefferenceNumberBody body)
        {
            var req = new PostTaxRefferenceNumber.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("TaxRefferenceNumber")]
        public async Task<IActionResult> UpdateTaxRefferenceNumber([FromBody] PutTaxRefferenceNumber.RequestTaxRefferenceUpdate body)
        {
            var req = new PutTaxRefferenceNumber.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("TaxRefferenceNumber")]
        public async Task<IActionResult> DeleteTaxRefferenceNumber([FromBody] DeleteTaxRefferenceNumber.RequestTaxRefferenceDelete body)
        {
            var req = new DeleteTaxRefferenceNumber.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
