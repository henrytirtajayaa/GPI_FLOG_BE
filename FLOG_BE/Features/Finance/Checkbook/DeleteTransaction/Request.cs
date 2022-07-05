using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Checkbook.DeleteTransaction
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyCheckbookTransactionDelete Body { get; set; }
    }

    public class RequestBodyCheckbookTransactionDelete
    {
        public Guid CheckbookTransactionId { get; set; }
    }
}
