using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FLOG_BE.Model.Companies.Entities
{
    public partial class VehicleType
    {
        public VehicleType()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid VehicleTypeId { get; set; }

        public string VehicleTypeCode { get; set; }

        public string VehicleTypeName { get; set; }

        public string VehicleCategory { get; set; }

        public bool Inactive { get; set; }

        #endregion

        #region Generated Relationships
        #endregion

    }
}
