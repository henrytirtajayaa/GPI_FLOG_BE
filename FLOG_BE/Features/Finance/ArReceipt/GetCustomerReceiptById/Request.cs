using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.ArReceipt.GetCustomerReceiptById
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public Guid ReceiptHeaderId { get; set; }
    }
}
