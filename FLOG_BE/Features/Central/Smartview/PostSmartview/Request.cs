using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.Smartview.PostSmartview
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyPostSmartView Body{ get; set; }
    }

    public class RequestBodyPostSmartView
{
        public string GroupName { get; set; }
        public string SmartTitle { get; set; }
        public string SqlViewName { get; set; }
        public bool isFunction { get; set; }
    }
}
