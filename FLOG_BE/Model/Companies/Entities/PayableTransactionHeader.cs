using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class PayableTransactionHeader
    {
        public PayableTransactionHeader()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid PayableTransactionId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string BranchCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public Guid VendorId { get; set; }
        [NotMapped]
        public string VendorName { get; set; }
        [NotMapped]
        public string VendorCode { get; set; }
        public string PaymentTermCode { get; set; }
        public string VendorAddressCode { get; set; }
        public string VendorDocumentNo { get; set; }
        public string NsDocumentNo { get; set; }
        [NotMapped]
        public string MasterNo { get; set; }
        [NotMapped]
        public string AgreementNo { get; set; }
        public string Description { get; set; }
        public decimal SubtotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string CreatedBy { get; set; }
        [NotMapped]
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        [NotMapped]
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string VoidBy { get; set; }
        [NotMapped]
        public string VoidByName { get; set; }
        public DateTime? VoidDate { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }
        public string BillToAddressCode { get; set; }
        public string ShipToAddressCode { get; set; }
        [NotMapped]
        public decimal OriginatingExtendedAmount { get; set; }
        [NotMapped]
        public List<PayableTransactionDetail> PayableTransactionDetails { get; set; }
        [NotMapped]
        public List<PayableTransactionTax> PayableTransactionTaxes { get; set; }
        [NotMapped]
        public int DecimalPlaces { get; set; }
        [NotMapped]
        public virtual SalesOrderHeader SalesOrder { get; set; }
        #endregion
    }
}
