using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class CustomerVendorRelation
    {
        public CustomerVendorRelation()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid RelationId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid VendorId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion
        public Customer Customers { get; set; }
        public Vendor Vendors { get; set; }

    }
}
