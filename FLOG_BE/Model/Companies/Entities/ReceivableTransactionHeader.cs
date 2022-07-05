using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class ReceivableTransactionHeader
    {
        public ReceivableTransactionHeader()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid ReceiveTransactionId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string BranchCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public Guid CustomerId { get; set; }
        [NotMapped]
        public string CustomerCode { get; set; }
        [NotMapped]
        public string CustomerName { get; set; }
        public string PaymentTermCode { get; set; }
        public string CustomerAddressCode { get; set; }
        public string SoDocumentNo { get; set; }
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
        public List<ReceivableTransactionDetail> ReceivableTransactionDetails { get; set; }
        [NotMapped]
        public List<ReceivableTransactionTax> ReceivableTransactionTaxes { get; set; }
        [NotMapped]
        public int DecimalPlaces { get; set; }
        #endregion


    }
}
