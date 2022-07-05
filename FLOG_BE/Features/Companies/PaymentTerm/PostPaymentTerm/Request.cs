using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.PaymentTerm.PostPaymentTerm
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPaymentTermBody Body { get; set; }
    }

    public class RequestPaymentTermBody
    {
        public string PaymentTermCode { get; set; }
        public string PaymentTermDesc { get; set; }
        public int? Due { get; set; }
        public int? Unit { get; set; }

    }
}
