using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.View 
{
    //FUNCTION : [dbo].[fxnBankActivities] (@checkbookCode AS VARCHAR(50), @trxDate DATE, @bankReconcileId UNIQUEIDENTIFIER)
    public class BankActivity
    {
        [Column("TransactionId")]
        public Guid TransactionId { get; set; }
        [Column("Modul", TypeName = "varchar")]
        public string Modul { get; set; }
        [Column("DocumentType", TypeName = "varchar")]
        public string DocumentType { get; set; }
        [Column("DocumentNo", TypeName = "varchar")]
        public string DocumentNo { get; set; }
        [Column("TransactionType", TypeName = "varchar")]
        public string TransactionType { get; set; }
        [Column("TransactionDate", TypeName = "date")]
        public DateTime TransactionDate { get; set; }
        [Column("CheckbookCode", TypeName = "varchar")]
        public string CheckbookCode { get; set; }
        [Column("CurrencyCode", TypeName = "varchar")]
        public string CurrencyCode { get; set; }
        [Column("ExchangeRate", TypeName = "decimal")]
        public decimal ExchangeRate { get; set; }
        [Column("IsMultiply", TypeName = "bit")]
        public bool IsMultiply { get; set; }
        [Column("SubjectCode", TypeName = "varchar")]
        public string SubjectCode { get; set; }
        [Column("Description", TypeName = "varchar")]
        public string Description { get; set; }
        [Column("BankAccountCode", TypeName = "varchar")]
        public string BankAccountCode { get; set; }
        [Column("OriginatingTotalAmount", TypeName = "decimal")]
        public decimal OriginatingTotalAmount { get; set; }
        [Column("OriginatingDebit", TypeName = "decimal")]
        public decimal OriginatingDebit { get; set; }
        [Column("OriginatingCredit", TypeName = "decimal")]
        public decimal OriginatingCredit { get; set; }
        [Column("IsChecked", TypeName = "bit")]
        public bool IsChecked { get; set; }
        [Column("Status", TypeName = "int")]
        public int Status { get; set; }
        [Column("BankReconcileId")]
        public Guid BankReconcileId { get; set; }
    }
}
