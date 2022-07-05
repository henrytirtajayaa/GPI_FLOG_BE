using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.APApply.PutApplyPayable
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPayment Body { get; set; }
    }

    public class RequestPayment
    {
        public Guid PayableApplyId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentType { get; set; }
        public Guid PaymentHeaderId { get; set; }
        public Guid CheckbookTransactionId { get; set; }
        public Guid PayableTransactionId { get; set; }
        public Guid VendorId { get; set; }
        public string Description { get; set; }
        public decimal OriginatingTotalPaid { get; set; }
        public decimal FunctionalTotalPaid { get; set; }

        public List<APApplyDetail> APApplyDetails { get; set; }
    }

    public class APApplyDetail
    {
        public Guid PayableTransactionId { get; set; }
        public string Description { get; set; }
        public decimal OriginatingBalance { get; set; }
        public decimal FunctionalBalance { get; set; }
        public decimal OriginatingPaid { get; set; }
        public decimal FunctionalPaid { get; set; }
        public int Status { get; set; }
    }

   
}
