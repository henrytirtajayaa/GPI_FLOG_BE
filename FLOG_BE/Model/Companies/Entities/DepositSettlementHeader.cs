using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class DepositSettlementHeader
    {
        public DepositSettlementHeader()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid SettlementHeaderId { get; set; }
        public Int64 RowId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public Guid ReceiveTransactionId { get; set; }
        public string DepositNo { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public string CheckbookCode { get; set; }
        public Guid CustomerId { get; set; }
        [NotMapped]
        public string CustomerCode { get; set; }
        [NotMapped]
        public string CustomerName { get; set; }
        public string Description { get; set; }
        public decimal OriginatingTotalPaid { get; set; }
        public decimal FunctionalTotalPaid { get; set; }
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
        public string ApplyDocumentNo { get; set; }
        public string CheckbookDocumentNo { get; set; }

        [NotMapped]
        public List<DepositSettlementDetail> DepositSettlementDetails { get; set; }

        [NotMapped]
        public decimal OriginatingTotalInvoice { get; set; }

        [NotMapped]
        public decimal AppliedTotalPaid { get; set; }
        #endregion
    }
}
