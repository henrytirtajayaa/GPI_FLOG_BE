﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.View 
{
    //VIEW : vw_pending_ap_credit_note
    public class APCreditNote
    {
        [Column("PayableTransactionId")]
        public Guid PayableTransactionId { get; set; }
        [Column("VendorId")]
        public Guid VendorId { get; set; }
        [Column("DocumentType")]
        public string DocumentType { get; set; }
        [Column("DocumentNo")]
        public string DocumentNo { get; set; }
        [Column("TransactionDate")]
        public DateTime TransactionDate { get; set; }
        
        [Column("VendorCode")]
        public string VendorCode { get; set; }

        [Column("VendorName")]
        public string VendorName { get; set; }
        
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
