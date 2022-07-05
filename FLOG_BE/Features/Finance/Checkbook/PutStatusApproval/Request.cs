using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Checkbook.PutStatusApproval
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutStatusCheckbookApproval Body { get; set; }
    }

    public class RequestPutStatusCheckbookApproval
    {
        public Guid CheckbookTransactionId { get; set; }

        public int ActionDocStatus { get; set; }

        public string Comments { get; set; }

        public Guid? PersonId { get; set; }
        public int CurrentIndex { get; set; }

    }

}
