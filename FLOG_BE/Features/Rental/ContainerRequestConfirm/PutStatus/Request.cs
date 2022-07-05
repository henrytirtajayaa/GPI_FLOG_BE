using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Rental.ContainerRequestConfirm.PutStatus
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutStatus Body { get; set; }
    }

    public class RequestPutStatus
    {
        public Guid ContainerRequestConfirmHeaderId { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int Status { get; set; }
    }
}
