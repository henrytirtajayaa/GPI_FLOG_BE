using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace FLOG_BE.Model.Companies.Entities
{
    public class ShippingLine
    {
        public ShippingLine()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid ShippingLineId { get; set; }
        public string ShippingLineCode { get; set; }
        public string ShippingLineName { get; set; }
        public string ShippingLineType { get; set; }
        public Guid VendorId { get; set; }
        public bool IsFeeder { get; set; }
        public bool Inactive { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion

        #region Generated Relationship
        [IgnoreDataMember]
        public virtual Vendor Vendor { get; set; }
        #endregion

    }
}
