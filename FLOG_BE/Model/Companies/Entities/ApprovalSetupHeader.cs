using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class ApprovalSetupHeader
    {
        public ApprovalSetupHeader()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        public Guid ApprovalSetupHeaderId { get; set; }
        public string ApprovalCode { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion


    }
}
