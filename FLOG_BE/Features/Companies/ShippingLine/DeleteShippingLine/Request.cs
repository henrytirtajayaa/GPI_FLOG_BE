using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ShippingLine.DeleteShippingLine
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestShippingLineDelete Body { get; set; }
    }

    public class RequestShippingLineDelete
    {
        public Guid ShippingLineId { get; set; }
    }
}
