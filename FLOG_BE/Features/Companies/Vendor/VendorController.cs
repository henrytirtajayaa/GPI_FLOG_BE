using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.Vendor
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class VendorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VendorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Vendor")]
        public async Task<IActionResult> GetVendor(
             [CommaSeparated] List<string> VendorId
            , [CommaSeparated] List<string> VendorCode
            , [CommaSeparated] List<string> VendorName
            , [CommaSeparated] List<string> AddressCode
            , [CommaSeparated] List<string> TaxRegistrationNo
            , [CommaSeparated] List<string> VendorTaxName
            , [CommaSeparated] List<string> VendorGroupCode
            , [CommaSeparated] List<string> PaymentTermCode
            , [CommaSeparated] bool? HasCreditLimit
            , [CommaSeparated] List<decimal?> CreditLimitMin
            , [CommaSeparated] List<decimal?> CreditLimitMax
            , [CommaSeparated] List<string> ShipToAddressCode
            , [CommaSeparated] List<string> BillToAddressCode
            , [CommaSeparated] List<string> TaxAddressCode
            , [CommaSeparated] List<string> PayableAccountNo
            , [CommaSeparated] List<string> AccruedPayableAccountNo
            , [CommaSeparated] bool? Inactive
            , [CommaSeparated] List<string> CreatedBy
            , [CommaSeparated] List<DateTime?> CreatedDateStart
            , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> ModifiedBy
            , [CommaSeparated] List<DateTime?> ModifiedDateStart
            , [CommaSeparated] List<DateTime?> ModifiedDateEnd
            , [CommaSeparated] List<DateTime?> ModifiedDate
            , [CommaSeparated] List<string> sort
            , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetVendor.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetVendor.RequestFilter
                {
                    VendorId = VendorId,
                    VendorCode = VendorCode,
                    VendorName = VendorName,
                    AddressCode = AddressCode,
                    TaxRegistrationNo = TaxRegistrationNo,
                    VendorTaxName = VendorTaxName,
                    VendorGroupCode = VendorGroupCode,
                    PaymentTermCode = PaymentTermCode,
                    HasCreditLimit = HasCreditLimit,
                    CreditLimitMin = CreditLimitMin,
                    CreditLimitMax = CreditLimitMax,
                    ShipToAddressCode = ShipToAddressCode,
                    BillToAddressCode = BillToAddressCode,
                    TaxAddressCode = TaxAddressCode,
                    PayableAccountNo = PayableAccountNo,
                    AccruedPayableAccountNo = AccruedPayableAccountNo,
                    Inactive = Inactive,
                    CreatedBy = CreatedBy,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
                    ModifiedDate = ModifiedDate
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("Vendor")]
        public async Task<IActionResult> CreateVendor([FromBody] PostVendor.RequestVendorBody body)
        {
           var req = new PostVendor.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("Vendor")]
        public async Task<IActionResult> DeleteVendor(DeleteVendor.RequestVendorDelete body)
        {
            var req = new DeleteVendor.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("Vendor")]
        public async Task<IActionResult> UpdateTaxSchedule([FromBody] PutVendor.RequestPutVendorBody body)
        {
            var req = new PutVendor.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
