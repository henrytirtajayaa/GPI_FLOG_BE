using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Finance.GLStatement
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class GLStatementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GLStatementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Category

        [HttpPost]
        [Route("GLStatementCtg")]
        public async Task<IActionResult> PostGLStatementCategory([FromBody] PostGLStatementCategory.RequestFormBody body)
        {
            var req = new PostGLStatementCategory.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("GLStatementCtg")]
        public async Task<IActionResult> UpdateGLStatementCategory([FromBody] PutGLStatementCategory.RequestFormBody body)
        {
            var req = new PutGLStatementCategory.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }        

        [HttpDelete]
        [Route("GLStatementCtg")]
        public async Task<IActionResult> DeleteGlStatementCategory([FromBody] DeleteGLStatementCategory.RequestDeleteBody body)
        {
            var req = new DeleteGLStatementCategory.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
       
        [HttpGet]
        [Route("GetGLStatementCtg")]
        public async Task<IActionResult> GetGLStatementCtg(
            [CommaSeparated] List<int> StatementType
           , [CommaSeparated] List<string> CategoryKey
           , [CommaSeparated] List<string> CategoryCaption
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetGLStatementCategory.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetGLStatementCategory.RequestFilter
                {
                    StatementType = StatementType,
                    CategoryKey = CategoryKey,
                    CategoryCaption = CategoryCaption
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        #endregion Category

        #region Statement Detail

        [HttpPost]
        [Route("GLStatementLayout")]
        public async Task<IActionResult> PostGLStatementDetail([FromBody] PostGLStatementDetail.RequestFormBody body)
        {
            var req = new PostGLStatementDetail.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("GLStatementLayout")]
        public async Task<IActionResult> UpdateGLStatementDetail([FromBody] PutGLStatementDetail.RequestFormBody body)
        {
            var req = new PutGLStatementDetail.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("GLStatementLayout")]
        public async Task<IActionResult> DeleteGLStatementDetail([FromBody] DeleteGLStatementDetail.RequestDeleteBody body)
        {
            var req = new DeleteGLStatementDetail.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetGLStatementLayout")]
        public async Task<IActionResult> GetGLStatementDetail(
            int StatementType
           , [CommaSeparated] List<string> AccountName
           , [CommaSeparated] List<string> SubCategoryKey
           , [CommaSeparated] List<string> SubCategoryCaption
           , [CommaSeparated] List<string> CategoryKey
           , [CommaSeparated] List<string> CategoryCaption
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetGLStatementDetail.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetGLStatementDetail.RequestFilter
                {
                    StatementType = StatementType,
                    AccountName = AccountName, 
                    SubCategoryKey = SubCategoryKey,
                    SubCategoryCaption = SubCategoryCaption,
                    CategoryKey = CategoryKey,
                    CategoryCaption = CategoryCaption
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        #endregion Statement Detail

        #region Statement

        [HttpGet]
        [Route("GetGLStatementPeriod")]
        public async Task<IActionResult> GetGLStatementPeriod()
        {
            var req = new GetGLStatementPeriod.Request();

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetReportStatementTB")]
        public async Task<IActionResult> GetReportStatementTB(
            [CommaSeparated] int PeriodYear, [CommaSeparated] int PeriodIndex, [CommaSeparated] string BranchCode, bool ShowZeroValue)
        {
            var req = new GetReportStatementTB.Request
            {
                Filter = new GetReportStatementTB.RequestFilter
                {
                    BranchCode = BranchCode,
                    PeriodIndex = PeriodIndex,
                    PeriodYear = PeriodYear,
                    ShowZeroValue = ShowZeroValue,
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        #endregion Statement
    }
}
