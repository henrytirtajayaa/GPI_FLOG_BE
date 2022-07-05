using Infrastructure.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Companies.PaymentTerm.DeletePaymentTerm
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyDeletePaymentTerm Body { get; set; }
    }

    public class RequestBodyDeletePaymentTerm
    {
        public string PaymentTermId { get; set; }
    }
}
