using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ApprovalSetup.DeleteApprovalSetup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyApprovalSetup Body { get; set; }
    }

    public class RequestBodyApprovalSetup
    { 
        public Guid ApprovalSetupHeaderId { get; set; }
    }
}
