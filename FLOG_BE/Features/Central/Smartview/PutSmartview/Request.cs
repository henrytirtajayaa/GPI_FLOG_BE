using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.Smartview.PutrSmartview
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyUpdateSmartview Body { get; set; }
    }

    public class RequestBodyUpdateSmartview
    {
        public Guid SmartViewId { get; set; }
        public string GroupName { get; set; }
        public string SmartTitle { get; set; }
        public string SqlViewName { get; set; }
        public bool isFunction { get; set; }

    }
}
