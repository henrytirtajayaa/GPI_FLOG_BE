using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.Smartview.GetDefaultSmartview
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public Guid SmartviewId { get; set; }
    }
}
