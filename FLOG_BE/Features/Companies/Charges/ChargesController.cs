using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.Charges
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class ChargesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChargesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Charges")]
        public async Task<IActionResult> GetCharges(
            [CommaSeparated] List<string> ChargesCode
            , [CommaSeparated] List<string> ChargeGroupCode
            , [CommaSeparated] List<string> ChargesName
            , [CommaSeparated] List<string> TransactionType
            , [CommaSeparated] bool? IsPurchasing
            , [CommaSeparated] bool? IsSales
            , [CommaSeparated] bool? IsInventory
            , [CommaSeparated] bool? IsFinancial
            , [CommaSeparated] bool? IsAsset
            , [CommaSeparated] bool? IsDeposit
            , [CommaSeparated] List<string> RevenueAccountNo
            , [CommaSeparated] List<string> TempRevenueAccountNo
            , [CommaSeparated] List<string> CostAccountNo
            , [CommaSeparated] List<string> TaxScheduleCode
            , [CommaSeparated] List<string> ShippingLineType
            , [CommaSeparated] bool? HasCosting
            , [CommaSeparated] bool? InActive
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
            var req = new GetCharges.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetCharges.RequestFilter
                {
                    ChargesCode = ChargesCode,
                    ChargeGroupCode = ChargeGroupCode,
                    ChargesName = ChargesName,
                    TransactionType = TransactionType,
                    IsPurchasing = IsPurchasing,
                    IsSales = IsSales,
                    IsInventory = IsInventory,
                    IsFinancial = IsFinancial,
                    IsAsset = IsAsset,
                    IsDeposit = IsDeposit,
                    RevenueAccountNo = RevenueAccountNo,
                    TempRevenueAccountNo = TempRevenueAccountNo,
                    CostAccountNo = CostAccountNo,
                    TaxScheduleCode = TaxScheduleCode,
                    ShippingLineType = ShippingLineType,
                    HasCosting = HasCosting,
                    InActive = InActive,
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

        [HttpGet]
        [Route("ChargesDeposit")]
        public async Task<IActionResult> GetChargesDeposit(
            [CommaSeparated] List<string> ChargesCode
            , [CommaSeparated] List<string> ChargeGroupCode
            , [CommaSeparated] List<string> ChargesName
            , [CommaSeparated] List<string> TransactionType
            , [CommaSeparated] bool? IsPurchasing
            , [CommaSeparated] bool? IsSales
            , [CommaSeparated] bool? IsInventory
            , [CommaSeparated] bool? IsFinancial
            , [CommaSeparated] bool? IsAsset
            , [CommaSeparated] bool? IsDeposit
            , [CommaSeparated] List<string> RevenueAccountNo
            , [CommaSeparated] List<string> TempRevenueAccountNo
            , [CommaSeparated] List<string> CostAccountNo
            , [CommaSeparated] List<string> TaxScheduleCode
            , [CommaSeparated] List<string> ShippingLineType
            , [CommaSeparated] bool? HasCosting
            , [CommaSeparated] bool? InActive
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd
            , [CommaSeparated] List<string> ModifiedBy

            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetChargesDeposit.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetChargesDeposit.RequestFilter
                {
                    ChargesCode = ChargesCode,
                    ChargeGroupCode = ChargeGroupCode,
                    ChargesName = ChargesName,
                    TransactionType = TransactionType,
                    IsPurchasing = IsPurchasing,
                    IsSales = IsSales,
                    IsInventory = IsInventory,
                    IsFinancial = IsFinancial,
                    IsAsset = IsAsset,
                    IsDeposit = IsDeposit,
                    RevenueAccountNo = RevenueAccountNo,
                    TempRevenueAccountNo = TempRevenueAccountNo,
                    CostAccountNo = CostAccountNo,
                    TaxScheduleCode = TaxScheduleCode,
                    ShippingLineType = ShippingLineType,
                    HasCosting = HasCosting,
                    InActive = InActive,
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

        [HttpPut]
        [Route("Charges")]
        public async Task<IActionResult> UpdateCharges([FromBody] PutCharges.RequestPutChargeBody body)
        {
            var req = new PutCharges.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("Charges")]
        public async Task<IActionResult> PostCharges([FromBody] PostCharges.RequestChargesBody body)
        {
            var req = new PostCharges.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("Charges")]
        public async Task<IActionResult> DeleteCharges(DeleteCharges.RequestChargesDelete body)
        {
            var req = new DeleteCharges.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
