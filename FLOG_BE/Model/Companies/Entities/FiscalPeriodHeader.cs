using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class FiscalPeriodHeader
    {
        public FiscalPeriodHeader()
        {
            #region Generated Constructor
            #endregion
        }
        #region Generated Properties

        public Guid FiscalHeaderId { get; set; }
        public int PeriodYear { get; set; }
        public int TotalPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool ClosingYear { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion

    }
}
