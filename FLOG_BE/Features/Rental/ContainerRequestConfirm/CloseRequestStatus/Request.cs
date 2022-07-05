using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Rental.ContainerRequestConfirm.CloseRequestStatus
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestCloseRental Body { get; set; }
    }

    public class RequestCloseRental
    {
        public Guid ContainerRentalRequestHeaderId { get; set; }
        public int Status { get; set; }
    }
}
