﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class CheckbookTransactionHeader
    {
        public CheckbookTransactionHeader()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid CheckbookTransactionId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string BranchCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public string CheckbookCode { get; set; }
        public bool IsVoid { get; set; }
        public string VoidDocumentNo { get; set; }
        public string PaidSubject { get; set; }
        public string SubjectCode { get; set; }
        public string Description { get; set; }
        public decimal OriginatingTotalAmount { get; set; }
        public decimal FunctionalTotalAmount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string VoidBy { get; set; }
        public DateTime? VoidDate { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }

        #endregion

        [NotMapped]
        public List<CheckbookTransactionDetail> CheckbookDetails{get;set;}

    }
}
