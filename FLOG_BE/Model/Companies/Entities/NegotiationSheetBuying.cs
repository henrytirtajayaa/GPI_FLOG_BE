using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class NegotiationSheetBuying
    {
        public NegotiationSheetBuying()
        {
            #region Generated Constructor
            #endregion
        }
   
        public Guid NsBuyingId { get; set; }
        public Int64 RowId { get; set; }
        public Guid NegotiationSheetId { get; set; }
        public Guid NsSellingId { get; set; }
        public Guid ChargeId { get; set; }        
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public decimal OriginatingAmount { get; set; }
        public decimal OriginatingTax { get; set; }
        public decimal OriginatingDiscount { get; set; }
        public decimal OriginatingExtendedAmount { get; set; }
        public decimal FunctionalTax { get; set; }
        public decimal FunctionalDiscount { get; set; }
        public decimal FunctionalExtendedAmount { get; set; }
        public Guid TaxScheduleId { get; set; }
        public bool IsTaxAfterDiscount { get; set; }
        public decimal PercentDiscount { get; set; }        
        public int PaymentCondition { get; set; }
        public Guid VendorId { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public int RowIndex { get; set; }

        public Guid PayableTransactionId { get; set; }

        public decimal UnitAmount { get; set; }
        public decimal Quantity { get; set; }

        [NotMapped]
        public string ChargeCode { get; set; }
        [NotMapped]
        public string ChargeName { get; set; }
        [NotMapped]
        public string ScheduleCode { get; set; }
        [NotMapped]
        public decimal TaxablePercentTax { get; set; }
        [NotMapped]
        public decimal DecimalPlaces { get; set; }
        [NotMapped]
        public string ChargeTo { get; set; }
        [NotMapped]
        public string PayableDocumentNo { get; set; }
    }
}
