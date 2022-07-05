using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class TaxRefferenceNumber
    {
        public TaxRefferenceNumber()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid TaxRefferenceId { get; set; }
        public DateTime StartDate { get; set; }
        public int ReffNoStart { get; set; }
        public int ReffNoEnd { get; set; }
        public int DocLength { get; set; }
        public int LastNo { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion
    }
}
