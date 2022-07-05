using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.ArReceipt.GetDetailCustomerReceipt
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    { 
        public Guid ReceiptHeaderId { get; set; }
        public Guid ReceiveTransactionId { get; set; }
        public string DocumentNo { get; set; }
    }
}
