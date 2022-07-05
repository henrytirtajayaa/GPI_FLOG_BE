using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Rental.ContainerRequestConfirm.PutDeleteContainerRequestConfirm
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutDeleteContainerRequestConfirm Body { get; set; }
    }

    public class RequestPutDeleteContainerRequestConfirm
    {
        public Guid ContainerRequestConfirmHeaderId { get; set; }
    }
}
