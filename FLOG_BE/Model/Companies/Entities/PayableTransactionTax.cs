using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class PayableTransactionTax
    {
        public PayableTransactionTax()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid TransactionTaxId { get; set; }
        public Guid PayableTransactionId { get; set; }
        public Guid TaxScheduleId { get; set; }
        public bool IsTaxAfterDiscount { get; set; }
        public string TaxScheduleCode { get; set; }
        public decimal TaxBasePercent { get; set; }
        public decimal TaxBaseOriginatingAmount { get; set; }
        public decimal TaxablePercent { get; set; }
        public decimal OriginatingTaxAmount { get; set; }
        public int Status { get; set; }
        #endregion
    }
}
