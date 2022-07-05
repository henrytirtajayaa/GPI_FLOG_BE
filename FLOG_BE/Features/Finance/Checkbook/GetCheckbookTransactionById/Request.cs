using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Checkbook.GetCheckbookTransactionById
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public Guid CheckbookTransactionId { get; set; }
    }
}
