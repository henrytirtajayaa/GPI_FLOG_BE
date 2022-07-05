using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.ApPayment.PutStatusApprovalPayment
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutStatusPaymentApproval Body { get; set; }
    }

    public class RequestPutStatusPaymentApproval
    {
        public Guid PaymentHeaderId { get; set; }

        public int ActionDocStatus { get; set; }

        public string Comments { get; set; }
     
        public Guid? PersonId { get; set; }
        public int CurrentIndex { get; set; }
        public DateTime ActionDate { get; set; }

    }

}
