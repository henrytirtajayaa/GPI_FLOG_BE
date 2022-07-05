using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.FiscalPeriodDetail
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class FiscalPeriodDetailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FiscalPeriodDetailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("FiscalPeriodDetail")]
        public async Task<IActionResult> GetFiscalPeriodHeader([CommaSeparated] List<Guid?> FiscalHeaderId
            , [CommaSeparated] List<int?> PeriodIndex
            , [CommaSeparated] List<DateTime?> PeriodStart
            , [CommaSeparated] List<DateTime?> PeriodEnd
            , [CommaSeparated] bool? IsClosePurchasing
            , [CommaSeparated] bool? IsCloseSales
            , [CommaSeparated] bool? IsCloseInventory
            , [CommaSeparated] bool? IsCloseFinancial
            , [CommaSeparated] bool? IsCloseAsset
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetFiscalPeriodDetail.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetFiscalPeriodDetail.RequestFilter
                {
                    FiscalHeaderId = FiscalHeaderId,
                    PeriodIndex = PeriodIndex,
                    PeriodEnd = PeriodEnd,
                    PeriodStart = PeriodStart,
                    IsCloseAsset = IsCloseAsset,
                    IsClosePurchasing = IsClosePurchasing,
                    IsCloseFinancial = IsCloseFinancial,
                    IsCloseInventory = IsCloseInventory,
                    IsCloseSales = IsCloseSales 
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("FiscalPeriodDetail")]
        public async Task<IActionResult> CreateFiscalPeriodHeader([FromBody] List<PostFiscalPeriodDetail.RequestFiscalDetailBody> body)
        {
            var req = new PostFiscalPeriodDetail.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("FiscalPeriodDetail")]
        public async Task<IActionResult> DeleteFiscalPeriodHeader(DeleteFiscalPeriodDetail.RequestFiscalDetailDelete body)
        {
            var req = new DeleteFiscalPeriodDetail.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

    }
}
