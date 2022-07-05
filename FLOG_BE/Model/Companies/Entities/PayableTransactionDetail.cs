using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class PayableTransactionDetail
    {
        public PayableTransactionDetail()
        {
            #region Generated Constructor
            #endregion
        }


        #region Generated Properties
        public Guid TransactionDetailId { get; set; }
        public Guid PayableTransactionId { get; set; }
        public Guid ChargesId { get; set; }
        [NotMapped]
        public string ChargesCode { get; set; }
        [NotMapped]
        public string ChargesName { get; set; }
        public string ChargesDescription { get; set; }
        public decimal OriginatingAmount { get; set; }
        public decimal OriginatingTax { get; set; }
        public decimal OriginatingDiscount { get; set; }
        public decimal OriginatingExtendedAmount { get; set; }
        public decimal FunctionalTax { get; set; }
        public decimal FunctionalDiscount { get; set; }
        public decimal FunctionalExtendedAmount { get; set; }
        public int Status { get; set; }
        public Guid TaxScheduleId { get; set; }
        public bool IsTaxAfterDiscount { get; set; }
        public decimal PercentDiscount { get; set; }

        public Int64 RowId { get; set; }

        [NotMapped]
        public decimal TaxablePercentTax { get; set; }

        [NotMapped]
        public string TaxScheduleCode { get; set; }

        #endregion
    }

}
