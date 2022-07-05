    using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.PaymentTerm.GetPaymentTerm
{
    public class Response
    {
        public List<ResponsePaymentTermItem> PaymentTerms { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponsePaymentTermItem
    {
        public string PaymentTermId { get; set; }
        public string PaymentTermCode { get; set; }
        public string PaymentTermDesc { get; set; }
        public int Due { get; set; }
        public int Unit { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
  
}
