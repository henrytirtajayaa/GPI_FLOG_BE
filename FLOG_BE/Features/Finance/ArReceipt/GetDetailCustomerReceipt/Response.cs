using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.ArReceipt.GetDetailCustomerReceipt
{
    public class Response
    {
        public List<ResponseItem> DetailReceipt { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid ReceiptDetailId { get; set; }
        public Int64 RowId { get; set; }
        public Guid ReceiptHeaderId { get; set; }
        public Guid ReceiveTransactionId { get; set; }
        public string Description { get; set; }
        public decimal OriginatingBalance { get; set; }
        public decimal FunctionalBalance { get; set; }
        public decimal OriginatingPaid { get; set; }
        public decimal FunctionalPaid { get; set; }
        public int Status { get; set; }
    }
}
