using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class BankReconcileDetail
    {
        public BankReconcileDetail()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid BankReconcileDetailId { get; set; }
        public Int64 RowId { get; set; }
        public Guid BankReconcileId { get; set; }
        public DateTime TransactionDate { get; set; } //Used to override to bank date (if necessary)
        public Guid TransactionId { get; set; }
        public string Modul { get; set; } //CHECKBOOK|RECEIPT|PAYMENT
        public int Status { get; set; }

        #endregion
        [NotMapped]
        public bool IsChecked { get; set; }
        [NotMapped]
        public string DocumentNo { get; set; }
        [NotMapped]
        public string DocumentType { get; set; }
        [NotMapped]
        public string TransactionType { get; set; }
        [NotMapped]
        public string PaidSubject { get; set; }
        [NotMapped]
        public decimal OriginatingAmount { get; set; }
    }
}
