using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.ArReceipt.PutStatusCustomerReceipt
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutStatus Body { get; set; }
    }

    public class RequestPutStatus
    { 
        public Guid ReceiptHeaderId { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }
        public DateTime ActionDate { get; set; }
    }
}
