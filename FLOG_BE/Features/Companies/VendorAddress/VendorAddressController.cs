using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.VendorAddress
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class VendorAddressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VendorAddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("VendorAddress")]
        public async Task<IActionResult> CreateVendorAddress([FromBody] PostVendorAddress.RequestBodyPostVendorAddress body)
        {
            var req = new PostVendorAddress.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("VendorAddress")]
        public async Task<IActionResult> GetVendorAddress(
            [CommaSeparated] List<string> VendorId
           , [CommaSeparated] List<string> VendorCode
           , [CommaSeparated] List<string> VendorName
           , [CommaSeparated] List<string> AddressCode
           , [CommaSeparated] List<string> AddressName
           , [CommaSeparated] List<string> ContactPerson
           , [CommaSeparated] List<string> Address
           , [CommaSeparated] List<string> Handphone
           , [CommaSeparated] List<string> Phone1
           , [CommaSeparated] List<string> Extension1
           , [CommaSeparated] List<string> Phone2
           , [CommaSeparated] List<string> Extension2
           , [CommaSeparated] List<string> Fax
           , [CommaSeparated] List<string> EmailAddress
           , [CommaSeparated] List<string> HomePage
           , [CommaSeparated] List<string> Neighbourhood
           , [CommaSeparated] List<string> Hamlet
           , [CommaSeparated] List<string> UrbanVillage
           , [CommaSeparated] List<string> CityCode
           , [CommaSeparated] List<string> CityName
           , [CommaSeparated] bool? Default
           , [CommaSeparated] List<string> CreatedBy
           , [CommaSeparated] List<DateTime?> CreatedDateStart
           , [CommaSeparated] List<DateTime?> CreatedDateEnd
            , [CommaSeparated] List<string> ModifiedBy
           , [CommaSeparated] List<DateTime?> ModifiedDateStart
           , [CommaSeparated] List<DateTime?> ModifiedDateEnd
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetVendorAddress.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetVendorAddress.RequestFilter
                {
                    VendorId = VendorId,
                    VendorCode = VendorCode,
                    VendorName = VendorName,
                    AddressCode = AddressCode,
                    AddressName = AddressName,
                    ContactPerson = ContactPerson,
                    Address = Address,
                    Handphone = Handphone,
                    Phone1 = Phone1,
                    Extension1 = Extension1,
                    Phone2 = Phone2,
                    Extension2 = Extension2,
                    Fax = Fax,
                    EmailAddress = EmailAddress,
                    HomePage = HomePage,
                    Neighbourhood = Neighbourhood,
                    Hamlet = Hamlet,
                    UrbanVillage = UrbanVillage,
                    CityCode = CityCode,
                    CityName = CityName,
                    Default = Default,
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

        [HttpDelete]
        [Route("VendorAddress")]
        public async Task<IActionResult> DeleteVendorAddress([FromBody] DeleteVendorAddress.RequestBodyDeleteVendorAddress body)
        {
            var req = new DeleteVendorAddress.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("VendorAddress")]
        public async Task<IActionResult> UpdateVendorAddress([FromBody] PutVendorAddress.RequestBodyUpdateVendorAddress body)
        {
            var req = new PutVendorAddress.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
