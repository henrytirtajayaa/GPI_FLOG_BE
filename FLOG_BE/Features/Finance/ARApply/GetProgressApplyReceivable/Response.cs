using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Features.Finance.ARApply.GetHistoryApplyReceivable;
using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Finance.ARApply.GetProgressApplyReceivable
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
        public string DocumentType { get; set; } /*ADVANCE | CREDIT NOTE */
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
    }

}
