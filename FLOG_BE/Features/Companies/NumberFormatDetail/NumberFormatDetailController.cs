using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.NumberFormatDetail
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class NumberFormatDetailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NumberFormatDetailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("NumberFormatDetail")]
        public async Task<IActionResult> GetNumberFormatDetail(
            [CommaSeparated] List<string> FormatHeaderId
            , [CommaSeparated] List<int?> SegmentNoMin
            , [CommaSeparated] List<int?> SegmentNoMax
            , [CommaSeparated] List<int?> SegmentTypeMin
            , [CommaSeparated] List<int?> SegmentTypeMax
            , [CommaSeparated] List<int?> SegmentLengthMin
            , [CommaSeparated] List<int?> SegmentLengthMax
            , [CommaSeparated] List<string> MaskFormat
            , [CommaSeparated] List<int?> StartingValueMin
            , [CommaSeparated] List<int?> StartingValueMax
            , [CommaSeparated] List<int?> EndingValueMin
            , [CommaSeparated] List<int?> EndingValueMax
            , [CommaSeparated] bool? Increase
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<string> CreatedByName
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetNumberFormatDetail.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetNumberFormatDetail.RequestFilter
                {
                    FormatHeaderId = FormatHeaderId,
                    SegmentNoMin = SegmentNoMin,
                    SegmentNoMax = SegmentNoMax,
                    SegmentTypeMin = SegmentTypeMin,
                    SegmentTypeMax = SegmentTypeMax,
                    SegmentLengthMin = SegmentLengthMin,
                    SegmentLengthMax = SegmentLengthMax,
                    MaskFormat = MaskFormat,
                    StartingValueMin = StartingValueMin,
                    StartingValueMax = StartingValueMax,
                    EndingValueMin = EndingValueMin,
                    EndingValueMax = EndingValueMax,
                    Increase = Increase
                }
            };

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("NumberFormatDetail")]
        public async Task<IActionResult> CreateNumberFormatDetail([FromBody] PostNumberFormatDetail.RequestBodyPostNumberFormatDetail body)
        {
            var req = new PostNumberFormatDetail.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("NumberFormatDetail")]
        public async Task<IActionResult> UpdateNumberFormatDetail([FromBody] PutNumberFormatDetail.RequestBodyUpdateNumberFormatDetail body)
        {
            var req = new PutNumberFormatDetail.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("NumberFormatDetail")]
        public async Task<IActionResult> DeleteNumberFormatDetail([FromBody] DeleteNumberFormatDetail.RequestBodyDeleteNumberFormatDetail body)
        {
            var req = new DeleteNumberFormatDetail.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
