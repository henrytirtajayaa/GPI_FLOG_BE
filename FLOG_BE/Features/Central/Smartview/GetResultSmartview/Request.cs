using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.Smartview.GetResultSmartview
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public Guid SmartviewId { get; set; }
        public List<RequestFilterSmartview> Filter { get; set; }
    }

    public class RequestFilterSmartview
    {
        public string Field { get; set; }
        public string Filter { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
    }
}
