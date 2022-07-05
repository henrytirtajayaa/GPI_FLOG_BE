using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.View 
{
    //VIEW : vw_pending_payable
    public class APPayablePending
    {
        [Column("PayableTransactionId")]
        public Guid PayableTransactionId { get; set; }
        [Column("DocumentType")]
        public string DocumentType { get; set; }
        [Column("DocumentNo")]
        public string DocumentNo { get; set; }
        [Column("TransactionDate")]
        public DateTime TransactionDate { get; set; }
        [Column("VendorId")]
        public Guid VendorId { get; set; }
        [Column("VendorCode")]
        public string VendorCode { get; set; }
        [Column("VendorDocumentNo")]
        public string VendorDocumentNo { get; set; }
        [Column("NSDocumentNo")]
        public string NsDocumentNo { get; set; }
        [NotMapped]
        public string MasterNo { get; set; }
        [NotMapped]
        public string AgreementNo { get; set; }
        [Column("VendorName")]
        public string VendorName { get; set; }
        [Column("TransactionType")]
        public string TransactionType { get; set; }
        [Column("CurrencyCode")]
        public string CurrencyCode { get; set; }
        [Column("ExchangeRate", TypeName = "decimal")]
        public decimal ExchangeRate { get; set; }
        [Column("IsMultiply")]
        public bool IsMultiply { get; set; }
        [Column("PaymentTermCode")]
        public string PaymentTermCode { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("OriginatingInvoice", TypeName = "decimal")]
        public decimal OriginatingInvoice { get; set; }
        [Column("OriginatingPaid", TypeName = "decimal")]
        public decimal OriginatingPaid { get; set; }
        [Column("OriginatingBalance", TypeName = "decimal")]
        public decimal OriginatingBalance { get; set; }
    }
}
