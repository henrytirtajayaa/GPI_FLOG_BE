using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.CompanySetup.PostCompanySetup
{
    public class Response
    {
        public string CompanySetupId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddressId { get; set; }
        public string TaxRegistrationNo { get; set; }
        public string CompanyTaxName { get; set; }
        public string CompanyLogo { get; set; }
        public string LogoImageUrl { get; set; }
    }


}
