﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class Checkbook
    {
        public Checkbook()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid CheckbookId { get; set; }
        public string CheckbookCode { get; set; }
        public string CheckbookName { get; set; }
        public string CheckbookAccountNo { get; set; }
        public string CurrencyCode { get; set; }
        public string BankCode { get; set; }
        public string BankAccountCode { get; set; }
        public bool HasCheckoutApproval { get; set; }
        public string ApprovalCode { get; set; }
        public string CheckbookInDocNo { get; set; }
        public string CheckbookOutDocNo { get; set; }
        public string ReceiptDocNo { get; set; }
        public string PaymentDocNo { get; set; }
        public string ReconcileDocNo { get; set; }
        public bool IsCash { get; set; }
        public bool InActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion

        #region Generated Relationship
        [NotMapped]
        public string BankName { get; set; }
        #endregion
    }
}
