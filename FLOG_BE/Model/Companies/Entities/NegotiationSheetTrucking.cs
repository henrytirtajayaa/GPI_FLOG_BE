using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class NegotiationSheetTrucking
    {
        public NegotiationSheetTrucking()
        {
            #region Generated Constructor
            #endregion
        }


        #region Generated Properties
       
        public Guid NsTruckingId { get; set; }
        public Int64 RowId { get; set; }
        public Guid NegotiationSheetId { get; set; }
        public Guid VehicleTypeId { get; set; }
        public string TruckloadTerm { get; set; }
        public Guid VendorId { get; set; }
        public int Qty { get; set; }
        public Guid UomDetailId { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public int RowIndex { get; set; }

        [NotMapped]
        public string VehicleTypeCode { get; set; }
        [NotMapped]
        public string VehicleTypeName { get; set; }

        #endregion
    }

}
