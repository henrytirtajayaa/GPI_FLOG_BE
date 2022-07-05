using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ContainerDepot.PutContainerDepot
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestContainerDepotUpdate Body { get; set; }
    }

    public class RequestContainerDepotUpdate
    {
        public Guid ContainerDepotId { get; set; }
        public string DepotCode { get; set; }
        public string DepotName { get; set; }
        public Guid OwnerVendorId { get; set; }
        public string CityCode { get; set; }
        public bool Inactive { get; set; }
    }
}
