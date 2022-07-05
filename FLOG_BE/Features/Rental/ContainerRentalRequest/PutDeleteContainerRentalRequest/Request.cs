using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Rental.ContainerRentalRequest.PutDeleteContainerRentalRequest
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutDeleteContainerRentalRequest Body { get; set; }
    }

    public class RequestPutDeleteContainerRentalRequest
    { 
        public Guid ContainerRentalRequestHeaderId { get; set; }
    }
}
