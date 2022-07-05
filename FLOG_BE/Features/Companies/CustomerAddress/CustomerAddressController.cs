using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.CustomerAddress
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class CustomerAddressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerAddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

      

        [HttpPost]
        [Route("CustomerAddress")]
        public async Task<IActionResult> CreateCustomerAddress([FromBody] PostCustomerAddress.RequestBodyPostCustomerAddress body)
        {
            var req = new PostCustomerAddress.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }



        [HttpGet]
        [Route("CustomerAddress")]
        public async Task<IActionResult> GetCustomerAddress(
                [CommaSeparated] List<string> CustomerId
              , [CommaSeparated] List<string> CustomerCode
              , [CommaSeparated] List<string> CustomerName
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
              , [CommaSeparated] List<string> SubDistrict
              , [CommaSeparated] List<string> CityCode
              , [CommaSeparated] List<string> CityName
              , [CommaSeparated] List<string> PostCode
              , [CommaSeparated] bool? IsSameAddress
              , [CommaSeparated] List<string> TaxAddressId
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
            var req = new GetCustomerAddress.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetCustomerAddress.RequestFilter
                {
                    CustomerId = CustomerId,
                    CustomerCode = CustomerCode,
                    CustomerName = CustomerName,
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
                    SubDistrict = SubDistrict,
                    CityCode = CityCode,
                    CityName = CityName,
                    PostCode = PostCode,
                    IsSameAddress = IsSameAddress,
                    TaxAddressId = TaxAddressId,
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


        [HttpPut]
        [Route("CustomerAddress")]
        public async Task<IActionResult> UpdateCustomerAddress([FromBody] PutCustomerAddress.RequestBodyUpdateCustomerAddress body)
        {
            var req = new PutCustomerAddress.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("CustomerAddress")]
        public async Task<IActionResult> DeleteCustomerAddress([FromBody] DeleteCustomerAddress.RequestBodyDeleteCustomerAddress body)
        {
            var req = new DeleteCustomerAddress.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
