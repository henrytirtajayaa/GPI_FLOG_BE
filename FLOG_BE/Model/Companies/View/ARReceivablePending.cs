using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.View 
{
    //VIEW : vw_pending_Receivable
    public class ARReceivablePending
    {
      
        [Column("ReceiveTransactionId", TypeName = "uniqueidentifier")]
        public Guid ReceiveTransactionId { get; set; }
        [Column("DocumentType", TypeName = "varchar(50)")]
        public string DocumentType { get; set; }
        [Column("DocumentNo", TypeName = "varchar(50)")]
        public string DocumentNo { get; set; }
        [Column("TransactionDate", TypeName = "datetime")]
        public DateTime TransactionDate { get; set; }
        [Column("CustomerId", TypeName = "uniqueidentifier")]
        public Guid CustomerId { get; set; }
        [Column("CustomerCode", TypeName = "varchar(50)")]
        public string CustomerCode { get; set; }
        [Column("CustomerName", TypeName = "varchar(250)")]
        public string CustomerName { get; set; }
        [Column("SODocumentNo", TypeName = "varchar(100)")]
        public string SODocumentNo { get; set; }
        [Column("NSDocumentNo", TypeName = "varchar(100)")]
        public string NSDocumentNo { get; set; }
        [Column("TransactionType", TypeName = "varchar(50)")]
        [NotMapped]
        public string MasterNo { get; set; }
        [NotMapped]
        public string AgreementNo { get; set; }
        public string TransactionType { get; set; }
        [Column("CurrencyCode", TypeName = "varchar(50)")]
        public string CurrencyCode { get; set; }
        [Column("ExchangeRate", TypeName = "decimal")]
        public decimal ExchangeRate { get; set; }
        [Column("IsMultiply", TypeName = "bit")]
        public bool IsMultiply { get; set; }
        [Column("Description", TypeName = "text")]
        public string Description { get; set; }
        [Column("OriginatingInvoice", TypeName = "decimal")]
        public decimal OriginatingInvoice { get; set; }
        [Column("OriginatingPaid", TypeName = "decimal")]
        public decimal OriginatingPaid { get; set; }
        [Column("OriginatingBalance", TypeName = "decimal")]
        public decimal OriginatingBalance { get; set; }
    }
}
