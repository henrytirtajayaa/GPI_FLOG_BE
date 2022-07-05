using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;
using Microsoft.AspNetCore.Http;

namespace FLOG_BE.Features.Companies.CompanySetup.PutCompanySetup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestCompanySetupUpdate Body { get; set; }
    }

    public class RequestCompanySetupUpdate
    {
        public string CompanySetupId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddressId { get; set; }
        public string TaxRegistrationNo { get; set; }
        public string CompanyTaxName { get; set; }
        public string CompanyLogo { get; set; }
        public IFormFile LogoImage { get; set; }
    }

}
