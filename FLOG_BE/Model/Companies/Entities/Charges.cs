using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class Charges
    {
        public Charges()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        public Guid ChargesId { get; set; }
        public string ChargesCode { get; set; }
        public string ChargesName { get; set; }
        public string TransactionType { get; set; }
        public string ChargeGroupCode { get; set; }
        public bool IsPurchasing { get; set; }
        public bool IsSales { get; set; }
        public bool IsInventory { get; set; }
        public bool IsFinancial { get; set; }
        public bool IsAsset { get; set; }
        public string RevenueAccountNo { get; set; }
        public string TempRevenueAccountNo { get; set; }
        public string CostAccountNo { get; set; }
        public string TaxScheduleCode { get; set; }
        public string ShippingLineType { get; set; }
        public bool HasCosting { get; set; }
        public bool InActive { get; set; }
        public bool IsDeposit { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion

        [NotMapped]
        public List<ChargesDetail> ChargesDetails { get; set; }
        [NotMapped]
        public string ChargeGroupName { get; set; }
    }
}
