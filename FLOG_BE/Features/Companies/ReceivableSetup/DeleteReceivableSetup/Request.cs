using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ReceivableSetup.DeleteReceivableSetup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyDelete Body { get; set; }
    }

    public class RequestBodyDelete
    {
        public Guid ReceivableSetupId { get; set; }
    }
}
