using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class PaymentTerm
    {
        public PaymentTerm()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid PaymentTermId { get; set; }
        public string PaymentTermCode { get; set; }
        public string PaymentTermDesc { get; set; }
        public int? Due { get; set; }
        public int? Unit { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion

        #region Generated Relationships    
      
        #endregion

    }
}
