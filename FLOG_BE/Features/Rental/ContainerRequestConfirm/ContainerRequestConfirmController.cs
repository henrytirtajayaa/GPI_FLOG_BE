using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;


namespace FLOG_BE.Features.Rental.ContainerRequestConfirm
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ContainerRequestConfirmController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContainerRequestConfirmController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetProgressContainerRequestConfirm")]
        public async Task<IActionResult> GetProgressContainerRequestConfirm(
            [CommaSeparated] Guid ContainerRentalRequestHeaderId
            , [CommaSeparated] List<string> TransactionType
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
            , [CommaSeparated] List<string> DeliveryOrderNo
            , [CommaSeparated] List<DateTime?> IssueDateStart
            , [CommaSeparated] List<DateTime?> IssueDateEnd
            , [CommaSeparated] List<DateTime?> ExpiredDateStart
            , [CommaSeparated] List<DateTime?> ExpiredDateEnd
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<string> CreatedByName
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> ModifiedBy
            , [CommaSeparated] List<string> ModifiedByName
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd

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
                    ContainerRentalRequestHeaderId = ContainerRentalRequestHeaderId,
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
                    DeliveryOrderNo = DeliveryOrderNo,
                    IssueDateStart = IssueDateStart,
                    IssueDateEnd = IssueDateEnd,
                    ExpiredDateStart = ExpiredDateStart,
                    ExpiredDateEnd = ExpiredDateEnd,
                    CreatedBy = CreatedBy,
                    CreatedByName = CreatedByName,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    Status = Status,
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetHistoryContainerRequestConfirm")]
        public async Task<IActionResult> GetHistoryContainerRequestConfirm(
            [CommaSeparated] Guid ContainerRentalRequestHeaderId
            , [CommaSeparated] List<string> TransactionType
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
            , [CommaSeparated] List<string> DeliveryOrderNo
            , [CommaSeparated] List<DateTime?> IssueDateStart
            , [CommaSeparated] List<DateTime?> IssueDateEnd
            , [CommaSeparated] List<DateTime?> ExpiredDateStart
            , [CommaSeparated] List<DateTime?> ExpiredDateEnd
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<string> CreatedByName
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> ModifiedBy
            , [CommaSeparated] List<string> ModifiedByName
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd

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
                    ContainerRentalRequestHeaderId = ContainerRentalRequestHeaderId,
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
                    DeliveryOrderNo = DeliveryOrderNo,
                    IssueDateStart = IssueDateStart,
                    IssueDateEnd = IssueDateEnd,
                    ExpiredDateStart = ExpiredDateStart,
                    ExpiredDateEnd = ExpiredDateEnd,
                    CreatedBy = CreatedBy,
                    CreatedByName = CreatedByName,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedByName = ModifiedByName,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    Status = Status,
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("GetQuantityRemaining")]
        public async Task<IActionResult> GetQuantityRemainingController(
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
            var req = new GetQuantityRemaining.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetQuantityRemaining.RequestFilter
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

        [HttpPost]
        [Route("ContainerRequestConfirm")]
        public async Task<IActionResult> CreateContainerRequestConfirm([FromBody] PostContainerRequestConfirm.RequestContainerRequestConfirm body)
        {
            var req = new PostContainerRequestConfirm.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("ContainerRequestConfirm")]
        public async Task<IActionResult> UpdateContainerRequestConfirm([FromBody] PutContainerRequestConfirm.RequestContainerRequestConfirm body)
        {
            var req = new PutContainerRequestConfirm.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutDeleteContainerRequestConfirm")]
        public async Task<IActionResult> PutDeleteContainerRequestConfirm([FromBody] PutDeleteContainerRequestConfirm.RequestPutDeleteContainerRequestConfirm body)
        {
            var req = new PutDeleteContainerRequestConfirm.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutStatusContainerRequestConfirm")]
        public async Task<IActionResult> PutStatusContainerRequestConfirm([FromBody] PutStatus.RequestPutStatus body)
        {
            var req = new PutStatus.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("PutCloseRequestStatus")]
        public async Task<IActionResult> CloseRequestStatus([FromBody] CloseRequestStatus.RequestCloseRental body)
        {
            var req = new CloseRequestStatus.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
