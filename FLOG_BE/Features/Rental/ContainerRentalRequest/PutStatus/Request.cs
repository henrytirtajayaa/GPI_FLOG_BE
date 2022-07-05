using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Rental.ContainerRentalRequest.PutStatus
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutStatus Body { get; set; }
    }

    public class RequestPutStatus
    { 
        public Guid ContainerRentalRequestHeaderId { get; set; }
        public int Status { get; set; }
    }
}
