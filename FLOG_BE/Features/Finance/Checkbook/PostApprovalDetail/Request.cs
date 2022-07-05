using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Checkbook.PostApprovalDetail
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestCheckbookApproval Body { get; set; }
    }

    public class RequestCheckbookApproval
    {
        public Guid CheckbookTransactionId { get; set; }
        public string CheckbookCode { get; set; }
    }
}
