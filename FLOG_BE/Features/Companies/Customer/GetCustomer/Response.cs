using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;
using System;

namespace FLOG_BE.Features.Companies.Customer.GetCustomer
{
    public class Response
    {
        public List<ResponseItem> Customers { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ContactPerson { get; set; }
        public string AddressName { get; set; }
        public string Address { get; set; }
        public string AddressCode { get; set; }
        public string TaxRegistrationNo { get; set; }
        public string CustomerTaxName { get; set; }
        public string CustomerGroupCode { get; set; }
        public string PaymentTermCode { get; set; }
        public bool HasCreditLimit { get; set; }
        public Decimal CreditLimit { get; set; }
        public string ShipToAddressCode { get; set; }
        public string BillToAddressCode { get; set; }
        public string TaxAddressCode { get; set; }
        public string ReceivableAccountNo { get; set; }
        public string AccruedReceivableAccountNo { get; set; }
        public bool Inactive { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
