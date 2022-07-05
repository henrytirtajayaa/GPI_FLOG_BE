using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Mediator;
using static Infrastructure.Mediator.ApiResultToActionResultMapper;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Attributes.CommaSeparated;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FLOG_BE.Features.Companies.CompanySetup
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]

    public class CompanySetupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanySetupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("CompanySetup")]
        public async Task<IActionResult> CreateCompanySetup([FromForm] PostCompanySetup.RequestCompanySetupBody body)
        {
            var req = new PostCompanySetup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpPut]
        [Route("CompanySetup")]
        public async Task<IActionResult> UpdateCompanySetup([FromForm] PutCompanySetup.RequestCompanySetupUpdate body)
        {
            var req = new PutCompanySetup.Request
            {
                Body = body
            };
            return ToActionResult(await _mediator.Send(req));
        }

        [HttpGet]
        [Route("CompanySetup")]

        public async Task<IActionResult> GetCompanySetup([CommaSeparated] List<string> CompanyName
           , [CommaSeparated] List<string> CompanyAddress
           , [CommaSeparated] List<string> TaxRegistrationNo
           , [CommaSeparated] List<string> CompanyTaxName
           , [CommaSeparated] List<string> CompanyLogo
           , [CommaSeparated] List<string> sort
           , [FromQuery] int limit = 100, [FromQuery] int offset = 0)
        {
            var req = new GetCompanySetup.Request
            {
                Offset = offset,
                Limit = limit,
                Sort = sort,
                Filter = new GetCompanySetup.RequestFilter
                {
                    CompanyName = CompanyName,
                    CompanyAddress = CompanyAddress,
                    TaxRegistrationNo = TaxRegistrationNo,
                    CompanyTaxName = CompanyTaxName,
                    CompanyLogo = CompanyLogo
                }
            };
            return ToActionResult(await _mediator.Send(req));
        }
    }
}