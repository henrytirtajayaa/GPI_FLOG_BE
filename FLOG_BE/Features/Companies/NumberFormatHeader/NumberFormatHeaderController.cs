using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.NumberFormatHeader
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class NumberFormatHeaderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NumberFormatHeaderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("NumberFormatHeader")]
        public async Task<IActionResult> GetNumberFormatHeader(
            [CommaSeparated] List<string> DocumentId
            , [CommaSeparated] List<string> Description
            , [CommaSeparated] List<string> LastGeneratedNo
            , [CommaSeparated] List<string> NumberFormat
            , [CommaSeparated] bool? InActive
            , [CommaSeparated] bool? IsMonthlyReset
            , [CommaSeparated] bool? IsYearlyReset
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
            var req = new GetNumberFormatHeader.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetNumberFormatHeader.RequestFilter
                {
                    DocumentId = DocumentId,
                    Description = Description,
                    LastGeneratedNo = LastGeneratedNo,
                    NumberFormat = NumberFormat,
                    InActive = InActive,
                    IsMonthlyReset = IsMonthlyReset,
                    IsYearlyReset = IsYearlyReset,
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
        [Route("NumberFormatHeader")]
        public async Task<IActionResult> CreateNumberFormatHeader([FromBody] PostNumberFormatHeader.RequestBodyPostNumberFormatHeader body)
        {
            var req = new PostNumberFormatHeader.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("NumberFormatHeader")]
        public async Task<IActionResult> UpdateNumberFormatHeader([FromBody] PutNumberFormatHeader.RequestBodyUpdateNumberFormatHeader body)
        {
            var req = new PutNumberFormatHeader.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("NumberFormatHeader")]
        public async Task<IActionResult> DeleteNumberFormatHeader([FromBody] DeleteNumberFormatHeader.RequestBodyDeleteNumberFormatHeader body)
        {
            var req = new DeleteNumberFormatHeader.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
