﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Companies.CompanyAddress.GetCompanyAddress
{
    public class Response
    {
        public List<ResponseItem> CompanyAddresses { get; set; }
        public ListInfo ListInfo { get; set; }
    }
    
    public class ResponseItem
    {
        public string CompanyAddressId { get; set; }
        public string AddressCode { get; set; }
        public string AddressName { get; set; }
        public string ContactPerson { get; set; }
        public string Address { get; set; }
        public string Handphone { get; set; }
        public string Phone1 { get; set; }
        public string Extension1 { get; set; }
        public string Phone2 { get; set; }
        public string Extension2 { get; set; }
        public string Fax { get; set; }
        public string EmailAddress { get; set; }
        public string HomePage { get; set; }
        public string Neighbourhood { get; set; }
        public string Hamlet { get; set; }
        public string UrbanVillage { get; set; }
        public string SubDistrict { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string Province { get; set; }
        public Guid CountryId { get; set; }
        public string CountryName { get; set; }
        public string PostCode { get; set; }
        public bool IsSameAddress { get; set; }
        public string TaxAddressId { get; set; }
        public string TaxAddressName { get; set; }
        public bool Default { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
