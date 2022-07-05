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

namespace FLOG_BE.Features.Companies.SetupContainerRental
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class SetupContainerRentalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SetupContainerRentalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("SetupContainerRental")]
        public async Task<IActionResult> GetSetupContainerRental(
        [CommaSeparated] List<string> TransactionType
        , [CommaSeparated] List<string> RequestDocNo
        , [CommaSeparated] List<string> DeliveryDocNo
        , [CommaSeparated] List<string> ClosingDocNo
        , [CommaSeparated] List<string> UomScheduleCode
        , [CommaSeparated] int? CustomerFreeUsageDays
        , [CommaSeparated] int? ShippingLineFreeUsageDays
        , [CommaSeparated] int? CntOwnerFreeUsageDays
        , [CommaSeparated] List<string> sort
        , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetSetupContainerRental.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetSetupContainerRental.RequestFilter
                {
                   TransactionType = TransactionType,
                   RequestDocNo = RequestDocNo,
                   DeliveryDocNo = DeliveryDocNo,
                   ClosingDocNo = ClosingDocNo,
                   UomScheduleCode = UomScheduleCode,
                   CustomerFreeUsageDays = CustomerFreeUsageDays,
                   ShippingLineFreeUsageDays = ShippingLineFreeUsageDays,
                   CntOwnerFreeUsageDays = CntOwnerFreeUsageDays
                }
            };

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("SetupContainerRental")]
        public async Task<IActionResult> CreateSetupContainerRental([FromBody] PostSetupContainerRental.RequestBodyPostSetupContainerRental body)
        {
            var req = new PostSetupContainerRental.Request
            { 
                Body = body
            };

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("SetupContainerRental")]
        public async Task<IActionResult> UpdateSetupContainerRental([FromBody] PutSetupContainerRental.RequestBodyUpdateSetupContainerRental body)
        {
            var req = new PutSetupContainerRental.Request
            {
                Body = body
            };

            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("SetupContainerRental")]
        public async Task<IActionResult> DeleteCheckbookTransaction(DeleteSetupContainerRental.RequestBodyDeleteSetupContainerRental body)
        {
            var req = new DeleteSetupContainerRental.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
