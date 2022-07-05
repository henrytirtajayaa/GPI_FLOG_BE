using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.CustomerAddress.GetCustomerAddress
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public List<string> CustomerId { get; set; }
        public List<string> CustomerCode { get; set; }
        public List<string> CustomerName { get; set; }
        public List<string> AddressCode { get; set; }
        public List<string> AddressName { get; set; }
        public List<string> ContactPerson { get; set; }
        public List<string> Address { get; set; }
        public List<string> Handphone { get; set; }
        public List<string> Phone1 { get; set; }
        public List<string> Extension1 { get; set; }
        public List<string> Phone2 { get; set; }
        public List<string> Extension2 { get; set; }
        public List<string> Fax { get; set; }
        public List<string> EmailAddress { get; set; }
        public List<string> HomePage { get; set; }
        public List<string> Neighbourhood { get; set; }
        public List<string> Hamlet { get; set; }
        public List<string> UrbanVillage { get; set; }
        public List<string> SubDistrict { get; set; }
        public List<string> CityCode { get; set; }
        public List<string> CityName { get; set; }
        public List<string> PostCode { get; set; }
        public bool? IsSameAddress { get; set; }
        public List<string> TaxAddressId { get; set; }
        public bool? Default { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
    }
}
