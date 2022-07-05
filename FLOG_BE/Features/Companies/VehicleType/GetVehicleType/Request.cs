using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.VehicleType.GetVehicleType
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public List<string> VehicleTypeCode { get; set; }
        public List<string> VehicleTypeName { get; set; }
        public List<string> VehicleCategory { get; set; }
        public bool? Inactive { get; set; }
    }
}
