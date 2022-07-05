using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class VendorGroup
    {
        public VendorGroup()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid VendorGroupId { get; set; }
        public string VendorGroupCode { get; set; }
        public string VendorGroupName { get; set; }
        //public string AddressCode { get; set; }
        public string PaymentTermCode { get; set; }
        public string PayableAccountNo { get; set; }
        public string AccruedPayableAccountNo { get; set; }
        public bool InActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion

        #region Generated Relationship
        #endregion
    }
}
