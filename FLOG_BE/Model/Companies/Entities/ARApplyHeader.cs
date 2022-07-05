using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class ARApplyHeader
    {
        public ARApplyHeader()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid ReceivableApplyId { get; set; }
        public Int64 RowId { get; set; }       
        public DateTime TransactionDate { get; set; }
        public Guid CustomerId { get; set; }
        public string DocumentType { get; set; }
        public Guid ReceiptHeaderId { get; set; }
        public Guid CheckbookTransactionId { get; set; }
        public Guid ReceiveTransactionId { get; set; }
        public string DocumentNo { get; set; }
        public string Description { get; set; }
        public decimal OriginatingTotalPaid { get; set; }
        public decimal FunctionalTotalPaid { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string VoidBy { get; set; }

        public DateTime? VoidDate { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }
        
        
        [NotMapped]
        public string CustomerCode { get; set; }
        [NotMapped]
        public string CustomerName { get; set; }

        [NotMapped]
        public string CurrencyCode { get; set; }
        [NotMapped]
        public decimal ExchangeRate { get; set; }
        [NotMapped]
        public bool IsMultiply { get; set; }
        [NotMapped]
        public string ReffDocumentNo { get; set; }

        [NotMapped]
        public string CreatedByName { get; set; }
        
        [NotMapped]
        public string ModifiedByName { get; set; }
        
        [NotMapped]
        public List<ARApplyDetail> ARApplyDetails { get; set; }

        [NotMapped]
        public decimal OriginatingTotalInvoice { get; set; }

        [NotMapped]
        public decimal AvailableBalance { get; set; }

        [NotMapped]
        public string VoidByName { get; set; }

        #endregion
    }
}
