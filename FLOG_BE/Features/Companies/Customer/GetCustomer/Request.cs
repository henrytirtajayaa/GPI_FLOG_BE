using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Customer.GetCustomer
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
        public Guid CustomerId { get; set; }
        public List<string> CustomerCode { get; set; }
        public List<string> CustomerName { get; set; }
        public List<string> AddressCode { get; set; }
        public List<string> TaxRegistrationNo { get; set; }
        public List<string> CustomerTaxName { get; set; }
        public List<string> CustomerGroupCode { get; set; }
        public List<string> PaymentTermCode { get; set; }
        public bool? HasCreditLimit { get; set; }
        public List<decimal?> CreditLimit { get; set; }
        public List<string> ShipToAddressCode { get; set; }
        public List<string> BillToAddressCode { get; set; }
        public List<string> TaxAddressCode { get; set; }
        public List<string> ReceivableAccountNo { get; set; }
        public List<string> AccruedReceivableAccountNo { get; set; }
        public bool? Inactive { get; set; }
        public List<int?> Status { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
    }
}
