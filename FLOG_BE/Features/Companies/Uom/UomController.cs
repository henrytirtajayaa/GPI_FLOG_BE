using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.Uom
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class UomController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UomController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region UOM Base

        [HttpGet]
        [Route("UomBase")]
        public async Task<IActionResult> GetUomBase([CommaSeparated] List<string> UomCode
           , [CommaSeparated] List<string> UomName
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetUomBase.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetUomBase.RequestFilter
                {
                    UomCode = UomCode,
                    UomName = UomName,
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }


        [HttpPut]
        [Route("UomBase")]
        public async Task<IActionResult> UpdateUomBase([FromBody] PutUomBase.RequestUpdate body)
        {
            var req = new PutUomBase.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }


        [HttpPost]
        [Route("UomBase")]
        public async Task<IActionResult> CreateUomBase([FromBody] PostUomBase.RequestUomBaseBody body)
        {
            var req = new PostUomBase.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("UomBase")]
        public async Task<IActionResult> DeleteUomBase(DeleteUomBase.RequestDelete body)
        {
            var req = new DeleteUomBase.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        #endregion UOM Base

        #region UOM Header

        [HttpGet]
        [Route("UomHeader")]
        public async Task<IActionResult> GetUomHeader([CommaSeparated] List<string> UomScheduleCode
           , [CommaSeparated] List<string> UomScheduleName
           , [CommaSeparated] List<string> UomBaseCode
           , [CommaSeparated] List<string> UomBaseName
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetUomHeader.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetUomHeader.RequestFilter
                {
                    UomScheduleCode = UomScheduleCode,
                    UomScheduleName = UomScheduleName,
                    UomBaseCode = UomBaseCode,
                    UomBaseName = UomBaseName,
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }


        [HttpPut]
        [Route("UomHeader")]
        public async Task<IActionResult> UpdateUomHeader([FromBody] PutUomHeader.RequestUomBody body)
        {
            var req = new PutUomHeader.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }


        [HttpPost]
        [Route("UomHeader")]
        public async Task<IActionResult> CreateUomHeader([FromBody] PostUomHeader.RequestUomBody body)
        {
            var req = new PostUomHeader.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("UomHeader")]
        public async Task<IActionResult> DeleteUomHeader(DeleteUomHeader.RequestDelete body)
        {
            var req = new DeleteUomHeader.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        #endregion UOM Header

    }
}
