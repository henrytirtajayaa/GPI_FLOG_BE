using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.VehicleType.DeleteVehicleType
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyVehicleDelete Body { get; set; }
    }

    public class RequestBodyVehicleDelete
    {
        public Guid VehicleTypeId { get; set; }
    }
}
