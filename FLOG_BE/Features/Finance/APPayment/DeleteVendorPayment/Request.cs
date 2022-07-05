using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.ApPayment.DeleteVendorPayment
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPaymentDeleteBody Body { get; set; }
    }

    public class RequestPaymentDeleteBody
    {
        public Guid PaymentHeaderId { get; set; }

    }

}
