using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.ApPayment.PostSubmitApprovalDetail
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPaymentApproval Body { get; set; }
    }

    public class RequestPaymentApproval
    {
        public Guid PaymentHeaderId { get; set; }
        public string CheckbookCode { get; set; }
    }
}
