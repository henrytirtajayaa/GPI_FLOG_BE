using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.SetupContainerRental.DeleteSetupContainerRental
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyDeleteSetupContainerRental Body { get; set; }
    }

    public class RequestBodyDeleteSetupContainerRental
    {
        public Guid SetupContainerRentalId { get; set; }
    }
}
