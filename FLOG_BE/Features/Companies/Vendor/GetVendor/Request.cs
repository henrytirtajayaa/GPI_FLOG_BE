using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Vendor.GetVendor
{ 
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public List<string> VendorId { get; set; }
        public List<string> VendorCode { get; set; }
        public List<string> VendorName { get; set; }
        public List<string> AddressCode { get; set; }
        public List<string> TaxRegistrationNo { get; set; }
        public List<string> VendorTaxName { get; set; }
        public List<string> VendorGroupCode { get; set; }
        public List<string> PaymentTermCode { get; set; }
        public bool? HasCreditLimit { get; set; }
        public List<decimal?> CreditLimitMin { get; set; }
        public List<decimal?> CreditLimitMax { get; set; }
        public List<string> ShipToAddressCode { get; set; }
        public List<string> BillToAddressCode { get; set; }
        public List<string> TaxAddressCode { get; set; }
        public List<string> PayableAccountNo { get; set; }
        public List<string> AccruedPayableAccountNo { get; set; }
        public bool? Inactive { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
        public List<DateTime?> ModifiedDate { get; set; }
    }
}
