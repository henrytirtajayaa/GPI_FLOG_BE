using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class Reference
    {
        public Reference()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid ReferenceId { get; set; }
        public string ReferenceType { get; set; }
        public string ReferenceCode { get; set; }
        public string ReferenceName { get; set; }
        public bool Inactive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion


    }
}
