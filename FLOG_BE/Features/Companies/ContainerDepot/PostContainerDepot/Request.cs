using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ContainerDepot.PostContainerDepot
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestContainerDepotBody Body { get; set; }
    }

    public class RequestContainerDepotBody
    {
        public string DepotCode { get; set; }
        public string DepotName { get; set; }
        public Guid OwnerVendorId { get; set; }
        public string CityCode { get; set; }
        public bool Inactive { get; set; }
   
    }
}
