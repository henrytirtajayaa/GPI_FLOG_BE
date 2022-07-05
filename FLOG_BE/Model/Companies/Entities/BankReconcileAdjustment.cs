using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class BankReconcileAdjustment
    {
        public BankReconcileAdjustment()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid BankReconcileAdjustmentId { get; set; }
        public Int64 RowId { get; set; }
        public Guid BankReconcileId { get; set; }
        public Guid CheckbookTransactionId { get; set; }
        public Guid TransactionDetailId { get; set; } //Checkbook Transaction Detail Id
        public string DocumentType { get; set; } //IN | OUT
        public string TransactionType { get; set; } //NORMAL
        public Guid ChargesId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsMultiply { get; set; }
        public decimal ExchangeRate { get; set; }
        public string PaidSubject { get; set; }
        public string Description { get; set; }
        public decimal OriginatingAmount { get; set; }
        public int Status { get; set; }

        #endregion

        [NotMapped]
        public string ChargesCode { get; set; }
        [NotMapped]
        public string ChargesDescription { get; set; }
        [NotMapped]
        public decimal FunctionalAmount { get; set; }
    }
}
