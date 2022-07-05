using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.APApply.PutStatusApplyPayable
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutStatus Body { get; set; }
    }

    public class RequestPutStatus
    {
        public Guid PayableApplyId { get; set; }

        public int ActionDocStatus { get; set; }

        public string Comments { get; set; }
        public DateTime ActionDate { get; set; }

    }

}
