using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Receivable.PutTrxDelete
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestTransactionDeleteBody Body { get; set; }
    }

    public class RequestTransactionDeleteBody
    {
        public Guid ReceiveTransactionId { get; set; }

    }

}
