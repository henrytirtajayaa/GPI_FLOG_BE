using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.Customer.PostCustomer
{
    public class Response
    {
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string AddressCode { get; set; }
        public string TaxRegistrationNo { get; set; }
        public string VendorTaxName { get; set; }
        public string VendorGroupCode { get; set; }
        public string PaymentTermCode { get; set; }
        public bool HasCreditLimit { get; set; }
        public Decimal CreditLimit { get; set; }
        public string ShipToAddressCode { get; set; }
        public string BillToAddressCode { get; set; }
        public string ReceivableAccountNo { get; set; }
        public string AccruedReceivableAccountNo { get; set; }
        public bool Inactive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Status { get; set; }
    }


}
