using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class BankReconcileHeader
    {
        public BankReconcileHeader()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid BankReconcileId { get; set; }
        public Int64 RowId { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid PrevBankReconcileId { get; set; }
        public string DocumentNo { get; set; }
        public string CheckbookCode { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime BankCutoffStart { get; set; }
        public DateTime BankCutoffEnd { get; set; }
        public string Description { get; set; }
        public decimal BankEndingOrgBalance { get; set; }
        public decimal CheckbookEndingOrgBalance { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string VoidBy { get; set; }
        public DateTime? VoidDate { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }

        [NotMapped]
        public string CreatedByName { get; set; }
        [NotMapped]
        public string ModifiedByName { get; set; }
        [NotMapped]
        public string VoidByName { get; set; }
        [NotMapped]
        public List<BankReconcileDetail> ReconcileDetails { get; set; }

        [NotMapped]
        public List<BankReconcileAdjustment> ReconcileAdjustments { get; set; }
        [NotMapped]
        public decimal BalanceDifference { get; set; }
        [NotMapped]
        public decimal PrevCheckbookBalance { get; set; }
        [NotMapped]
        public string PrevReconcileDocNo { get; set; }
        [NotMapped]
        public bool AllowVoid { get; set; }
        #endregion
    }
}
