using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class ContainerDepot
    {
        public ContainerDepot()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        public Guid ContainerDepotId { get; set; }
        public string DepotCode { get; set; }
        public string DepotName { get; set; }
        public Guid OwnerVendorId { get; set; }
        public string CityCode { get; set; }
        public bool InActive { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion

        #region Generated Relationships
        public City Cities { get; set; }

        [NotMapped]
        public string VendorCode { get; set; }

        [NotMapped]
        public string VendorName { get; set; }
        #endregion

    }
}
