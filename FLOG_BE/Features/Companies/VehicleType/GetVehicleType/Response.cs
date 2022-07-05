using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.VehicleType.GetVehicleType
{
    public class Response
    {
        public List<ReponseItem> VehicleTypes { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ReponseItem
    {
        public Guid VehicleTypeId { get; set; }
        public string VehicleTypeCode { get; set; }
        public string VehicleTypeName { get; set; }
        public string VehicleCategory { get; set; }
        public bool Inactive { get; set; }

    }
}
