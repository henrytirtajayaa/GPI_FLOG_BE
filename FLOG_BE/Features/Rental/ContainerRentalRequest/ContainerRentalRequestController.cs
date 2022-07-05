using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Rental.ContainerRentalRequest
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ContainerRentalRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContainerRentalRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetProgressContainerRentalRequest")]
        public async Task<IActionResult> GetProgressContainerRentalRequestController(
            [CommaSeparated] List<string> TransactionType
            , [CommaSeparated] List<DateTime?> DocumentDateStart
            , [CommaSeparated] List<DateTime?> DocumentDateEnd
            , [CommaSeparated] List<string> DocumentNo
            , [CommaSeparated] Guid CustomerId
            , [CommaSeparated] List<string> CustomerCode
            , [CommaSeparated] List<string> CustomerName
            , [CommaSeparated] List<string> AddressCode
            , [CommaSeparated] int? Status
            , [CommaSeparated] List<string> SalesCode
            , [CommaSeparated] Guid VendorId
            , [CommaSeparated] List<string> VendorCode
            , [CommaSeparated] List<string> VendorName
            , [CommaSeparated] List<string> ShipToAddressCode
            , [CommaSeparated] List<string> BillToAddressCode
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<string> CreatedByName
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> ModifiedBy
            , [CommaSeparated] List<string> ModifiedByName
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd
            , [CommaSeparated] List<string> CanceledBy
            , [CommaSeparated] List<string> CanceledByName
            , [CommaSeparated] List<DateTime?> CanceledDateStart
            , [CommaSeparated] List<DateTime?> CanceledDateEnd

           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetProgress.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetProgress.RequestFilter
                {
                    TransactionType = TransactionType,
                    DocumentDateStart = DocumentDateStart,
                    DocumentDateEnd = DocumentDateEnd,
                    DocumentNo = DocumentNo,
                    CustomerId = CustomerId,
                    CustomerCode = CustomerCode,
                    CustomerName = CustomerName,
                    AddressCode = AddressCode,
                    SalesCode = SalesCode,
                    VendorId = VendorId,
                    VendorCode = VendorCode,
                    VendorName = VendorName,
                    BillToAddressCode = BillToAddressCode,
                    ShipToAddressCode = ShipToAddressCode,
                    CreatedBy = CreatedBy,
                    CreatedByName = CreatedByName,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    CanceledBy = CanceledBy,
                    CanceledByName = CanceledByName,
                    CanceledDateStart = CanceledDateStart,
                    CanceledDateEnd = CanceledDateEnd,
                    Status = Status,
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetHistoryContainerRentalRequest")]
        public async Task<IActionResult> GetHistoryContainerRentalRequestController(
            [CommaSeparated] List<string> TransactionType
            , [CommaSeparated] List<DateTime?> DocumentDateStart
            , [CommaSeparated] List<DateTime?> DocumentDateEnd
            , [CommaSeparated] List<string> DocumentNo
            , [CommaSeparated] Guid CustomerId
            , [CommaSeparated] List<string> CustomerCode
            , [CommaSeparated] List<string> CustomerName
            , [CommaSeparated] List<string> AddressCode
            , [CommaSeparated] int? Status
            , [CommaSeparated] List<string> SalesCode
            , [CommaSeparated] Guid VendorId
            , [CommaSeparated] List<string> VendorCode
            , [CommaSeparated] List<string> VendorName
            , [CommaSeparated] List<string> ShipToAddressCode
            , [CommaSeparated] List<string> BillToAddressCode
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<string> CreatedByName
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> ModifiedBy
            , [CommaSeparated] List<string> ModifiedByName
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd
            , [CommaSeparated] List<string> CanceledBy
            , [CommaSeparated] List<string> CanceledByName
            , [CommaSeparated] List<DateTime?> CanceledDateStart
            , [CommaSeparated] List<DateTime?> CanceledDateEnd

           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetHistory.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetHistory.RequestFilter
                {
                    TransactionType = TransactionType,
                    DocumentDateStart = DocumentDateStart,
                    DocumentDateEnd = DocumentDateEnd,
                    DocumentNo = DocumentNo,
                    CustomerId = CustomerId,
                    CustomerCode = CustomerCode,
                    CustomerName = CustomerName,
                    AddressCode = AddressCode,
                    SalesCode = SalesCode,
                    VendorId = VendorId,
                    VendorCode = VendorCode,
                    VendorName = VendorName,
                    ShipToAddressCode = ShipToAddressCode,
                    BillToAddressCode = BillToAddressCode,
                    CreatedBy = CreatedBy,
                    CreatedByName = CreatedByName,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    CanceledBy = CanceledBy,
                    CanceledByName = CanceledByName,
                    CanceledDateStart = CanceledDateStart,
                    CanceledDateEnd = CanceledDateEnd,
                    Status = Status,
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("ContainerRentalRequest")]
        public async Task<IActionResult> CreateContainerRentalRequest([FromBody] PostContainerRentalRequest.RequestContainerRentalRequest body)
        {
            var req = new PostContainerRentalRequest.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("ContainerRentalRequest")]
        public async Task<IActionResult> UpdateContainerRentalRequest([FromBody] PutContainerRentalRequest.RequestContainerRentalRequest body)
        {
            var req = new PutContainerRentalRequest.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutDeleteContainerRentalRequest")]
        public async Task<IActionResult> PutDeleteContainerRentalRequest([FromBody] PutDeleteContainerRentalRequest.RequestPutDeleteContainerRentalRequest body)
        {
            var req = new PutDeleteContainerRentalRequest.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutStatusContainerRentalRequest")]
        public async Task<IActionResult> PutStatusContainerRentalRequest([FromBody] PutStatus.RequestPutStatus body)
        {
            var req = new PutStatus.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
