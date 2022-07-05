using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.VehicleType.PutVehicleType
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyUpdateVehicleType Body { get; set; }
    }

    public class RequestBodyUpdateVehicleType
    {
        public Guid VehicleTypeId { get; set; }
        public string VehicleTypeCode { get; set; }
        public string VehicleTypeName { get; set; }
        public string VehicleCategory { get; set; }
        public bool Inactive { get; set; }
    }
}
