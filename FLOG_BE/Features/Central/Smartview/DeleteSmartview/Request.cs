using Infrastructure.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Central.Smartview.DeleteSmartview
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set;  }

        public RequestBodyDeleteSmartview Body { get; set; }
    }

    public class RequestBodyDeleteSmartview
    { 
        public Guid SmartviewId { get; set; }
    }
}
