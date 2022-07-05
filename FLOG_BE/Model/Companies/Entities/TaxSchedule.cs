using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class TaxSchedule
    {
        public TaxSchedule()
        {
            #region Generated Constructor
            #endregion
        }
       
        #region Generated Properties
        public Guid TaxScheduleId { get; set; }
        public string TaxScheduleCode { get; set; }  
        public string Description { get; set; }
        public bool IsSales { get; set; }
        public decimal PercentOfSalesPurchase { get; set; }
        public decimal TaxablePercent { get; set; }
        public byte RoundingType { get; set; }
        public decimal RoundingLimitAmount { get; set; }
        public bool TaxInclude { get; set; }
        public bool WithHoldingTax { get; set; }
        public string TaxAccountNo { get; set; }
        public bool Inactive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion


    }
}
