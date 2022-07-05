using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Vendor.PutVendor
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutVendorBody Body { get; set; }
    }

    public class RequestPutVendorBody
    {
        public Guid VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string AddressCode { get; set; }
        public string TaxRegistrationNo { get; set; }
        public string VendorTaxName { get; set; }
        public string VendorGroupCode { get; set; }
        public string PaymentTermCode { get; set; }
        public bool HasCreditLimit { get; set; }
        public decimal CreditLimit { get; set; }
        public string ShipToAddressCode { get; set; }
        public string BillToAddressCode { get; set; }
        public string TaxAddressCode { get; set; }
        public string PayableAccountNo { get; set; }
        public string AccruedPayableAccountNo { get; set; }
        public bool Inactive { get; set; }

    }
}
