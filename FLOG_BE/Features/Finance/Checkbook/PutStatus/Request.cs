using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Checkbook.PutStatus
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutStatusCheckbook Body { get; set; }
    }

    public class RequestPutStatusCheckbook
    {
        public Guid CheckbookTransactionId { get; set; }

        public int ActionDocStatus { get; set; }
        public DateTime ActionDate { get; set; }

        public string Comments { get; set; }

    }

}
