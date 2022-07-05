using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class AccountSegment
    {
        public AccountSegment()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public string SegmentId { get; set; }
        public int SegmentNo { get; set; }
        public string Description { get; set; }
        public int Length { get; set; }
        public bool IsMainAccount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion


    }
}
