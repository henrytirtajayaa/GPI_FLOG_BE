using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Customer.PostCustomer
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPostBody Body { get; set; }
    }

    public class RequestPostBody
    {
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
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
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Status { get; set; }

    }
}
