using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Receivable.GetReceivableTransactionById
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public Guid ReceiveTransactionId { get; set; }
    }
}
