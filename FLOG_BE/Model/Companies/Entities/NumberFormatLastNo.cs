using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class NumberFormatHeader
    {
        public NumberFormatHeader()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid FormatHeaderId { get; set; }
        public string DocumentId { get; set; }
        public string Description { get; set; }
        public string LastGeneratedNo { get; set; }
        public string NumberFormat { get; set; }
        public bool InActive { get; set; }
        public bool IsMonthlyReset { get; set; }
        public bool IsYearlyReset { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion

        #region Generated Relationships
        public virtual List<NumberFormatDetail> NumberFormatDetails { get; set; }
        #endregion
    }
}
