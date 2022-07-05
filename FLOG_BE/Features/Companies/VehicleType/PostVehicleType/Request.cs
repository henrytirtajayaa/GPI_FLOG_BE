using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.VehicleType.PostVehicleType
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string VehicleTypeCode { get; set; }
        public string VehicleTypeName { get; set; }
        public string VehicleCategory { get; set; }
        public bool Inactive { get; set; }
    }
}
