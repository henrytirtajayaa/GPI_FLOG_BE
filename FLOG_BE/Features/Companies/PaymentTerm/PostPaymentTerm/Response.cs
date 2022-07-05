using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.PaymentTerm.PostPaymentTerm
{
    public class Response
    {
        public string PaymentTermId { get; set; }
        public string PaymentTermCode { get; set; }
        public string PaymentTermDesc { get; set; }
        public int Due { get; set; }
        public int Unit { get; set; }

    }


}
