using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Payable.PutTrxDeletePayable
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestTransactionDeletePayable Body { get; set; }
    }

    public class RequestTransactionDeletePayable
    { 
        public Guid PayableTransactionId { get; set; }
    }
}
