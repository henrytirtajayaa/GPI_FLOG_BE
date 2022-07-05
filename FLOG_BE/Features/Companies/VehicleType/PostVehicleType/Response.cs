using System;
using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.VehicleType.PostVehicleType
{
    public class Response
    {
        public Guid VehicleTypeId { get; set; }

        public string VehicleTypeName { get; set; }
    }


}
