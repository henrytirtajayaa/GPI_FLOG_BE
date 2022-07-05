using Infrastructure.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Companies.CustomerAddress.PostCustomerAddress
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyPostCustomerAddress Body { get; set; }
    }

    public class RequestBodyPostCustomerAddress
    {
        public string CustomerId { get; set; }
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
        public string PostCode { get; set; }
        public bool IsSameAddress { get; set; }
        public string TaxAddressId { get; set; }
        public bool Default { get; set; }
    }
}
