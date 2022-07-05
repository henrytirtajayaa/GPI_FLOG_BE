using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Charges.DeleteCharges
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestChargesDelete Body { get; set; }
    }

    public class RequestChargesDelete
    {
        public String ChargesId { get; set; }
    }
}
