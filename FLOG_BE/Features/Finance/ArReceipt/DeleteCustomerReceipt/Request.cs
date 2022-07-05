using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.ArReceipt.DeleteCustomerReceipt
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestDeleteCustomerReceipt Body { get; set; }
    }
    
    public class RequestDeleteCustomerReceipt
    { 
        public Guid ReceiptHeaderId { get; set; }
    }
}
