using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using System.Threading.Tasks;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace FLOG_BE.Features.Companies.CompanyAddress
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class CompanyAddressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyAddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("CompanyAddress")]
        public async Task<IActionResult> GetCompanyAddress(
            [CommaSeparated] List<string> AddressCode
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
            var req = new GetCompanyAddress.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetCompanyAddress.RequestFilter
                {
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
                    PostCode = PostCode,
                    IsSameAddress = IsSameAddress,
                    TaxAddressId = TaxAddressId,
                    Default = Default,
                    CreatedBy = CreatedBy,
                    CreatedDateStart = CreatedDateStart,
                    CreatedDateEnd = CreatedDateEnd,
                    ModifiedBy = ModifiedBy,
                    ModifiedDateStart = ModifiedDateStart,
                    ModifiedDateEnd = ModifiedDateEnd,
    }
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPost]
        [Route("CompanyAddress")]
        public async Task<IActionResult> CreateCompanyAddress([FromBody] PostCompanyAddress.RequestBodyPostCompanyAddress body)
        {
            var req = new PostCompanyAddress.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("CompanyAddress")]
        public async Task<IActionResult> UpdateCompanyAddress([FromBody] PutCompanyAddress.RequestBodyUpdateCompanyAddress body)
        {
            var req = new PutCompanyAddress.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpDelete]
        [Route("CompanyAddress")]
        public async Task<IActionResult> DeleteCompanyAddress([FromBody] DeleteCompanyAddress.RequestBodyDeleteCompanyAddress body)
        {
            var req = new DeleteCompanyAddress.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}
