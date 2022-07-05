using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ContainerDepot.DeleteContainerDepot
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestContainerDepotDelete Body { get; set; }
    }

    public class RequestContainerDepotDelete
    {
        public Guid ContainerDepotId { get; set; }
    }
}
