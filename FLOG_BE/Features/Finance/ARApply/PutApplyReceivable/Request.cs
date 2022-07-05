using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.ARApply.PutApplyReceivable
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPayment Body { get; set; }
    }

    public class RequestPayment
    {
        public Guid ReceivableApplyId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentType { get; set; }
        public Guid ReceiptHeaderId { get; set; }
        public Guid CheckbookTransactionId { get; set; }
        public Guid ReceiveTransactionId { get; set; }
        public Guid CustomerId { get; set; }
        public string Description { get; set; }
        public decimal OriginatingTotalPaid { get; set; }
        public decimal FunctionalTotalPaid { get; set; }

        public List<ARApplyDetail> ARApplyDetails { get; set; }
    }

    public class ARApplyDetail
    {
        public Guid ReceiveTransactionId { get; set; }
        public string Description { get; set; }
        public decimal OriginatingBalance { get; set; }
        public decimal FunctionalBalance { get; set; }
        public decimal OriginatingPaid { get; set; }
        public decimal FunctionalPaid { get; set; }
        public int Status { get; set; }
    }

   
}
