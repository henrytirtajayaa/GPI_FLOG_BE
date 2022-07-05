using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOG_BE.Features.Finance.APApply.PostApplyPayable
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPaymentBody Body { get; set; }
    }

    public class RequestPaymentBody
    {
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
