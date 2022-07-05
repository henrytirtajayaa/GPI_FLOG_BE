using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.View 
{
    //VIEW : vw_pending_ar_advance
    public class ARAdvanceReceipt
    {
        [Column("CheckbookTransactionId")]
        public Guid CheckbookTransactionId { get; set; }
        [Column("CustomerId")]
        public Guid CustomerId { get; set; }
        [Column("CustomerCode")]
        public string CustomerCode { get; set; }
        [Column("CustomerName")]
        public string CustomerName { get; set; }
        [Column("DocumentType")]
        public string DocumentType { get; set; }
        [Column("TransactionType")]
        public string TransactionType { get; set; }
        [Column("DocumentNo")]
        public string DocumentNo { get; set; }
        [Column("TransactionDate")]
        public DateTime TransactionDate { get; set; }
        [Column("CheckbookCode")]
        public string CheckbookCode { get; set; }
        [Column("CurrencyCode")]
        public string CurrencyCode { get; set; }
        [Column("ExchangeRate", TypeName = "decimal")]
        public decimal ExchangeRate { get; set; }
        [Column("IsMultiply")]
        public bool IsMultiply { get; set; }        
        [Column("OriginatingAmount", TypeName = "decimal")]
        public decimal OriginatingAmount { get; set; }
        [Column("OriginatingPaid", TypeName = "decimal")]
        public decimal OriginatingPaid { get; set; }
        [Column("OriginatingBalance", TypeName = "decimal")]
        public decimal OriginatingBalance { get; set; }
    }
}
