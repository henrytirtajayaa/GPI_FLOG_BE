using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Port.DeletePort
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPortDelete Body { get; set; }
    }

    public class RequestPortDelete
    {
        public string PortId { get; set; }
    }
}
