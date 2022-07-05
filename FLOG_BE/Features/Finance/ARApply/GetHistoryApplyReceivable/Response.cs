using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Finance.ARApply.GetHistoryApplyReceivable
{
    public class Response
    {
        public List<ResponseItem> ReceivableApplies { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid ReceivableApplyId { get; set; }
        public Int64 RowId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentType { get; set; } /*ADVANCE | CREDIT NOTE*/
        public Guid CheckbookTransactionId { get; set; }
        public Guid ReceiveTransactionId { get; set; }
        public Guid ReceiptHeaderId { get; set; }
        public string DocumentNo { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public string ReffDocumentNo { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
        public decimal OriginatingTotalPaid { get; set; }
        public decimal FunctionalTotalPaid { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<ResponseDetail> ARApplyDetails { get; set; }

        public decimal OriginatingTotalInvoice { get; set; }

        public decimal AvailableBalance { get; set; }

        public string VoidBy { get; set; }
        public string VoidByName { get; set; }
        public DateTime? VoidDate { get; set; }
        public string StatusComment { get; set; }

    }
    public class ResponseDetail
    {
        public Guid ReceivableApplyId { get; set; }
        public Guid ReceiveTransactionId { get; set; }
        public string DocumentNo { get; set; }
        public string CustomerCode { get; set; }
        public string CurrencyCode { get; set; }
        public string SoDocumentNo { get; set; }
        public string NsDocumentNo { get; set; }
        public decimal OrgDocAmount { get; set; }
        public string Description { get; set; }
        public decimal OriginatingInvoice { get; set; }
        public decimal OriginatingBalance { get; set; }
        public decimal FunctionalBalance { get; set; }
        public decimal OriginatingPaid { get; set; }
        public decimal FunctionalPaid { get; set; }
        public int Status { get; set; }
    }

}
