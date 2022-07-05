using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class FinancialSetup
    {
        public FinancialSetup()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid FinancialSetupId { get; set; }
        public string FuncCurrencyCode { get; set; }
        public int DefaultRateType { get; set; }
        public int TaxRateType { get; set; }
        public int DeptSegmentNo { get; set; }
        public string CheckbookChargesType { get; set; }

        public string UomScheduleCode { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion

        #region Generated Relationships
        #endregion
    }
}
