using Infrastructure.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Central.Smartview.GetSmartview
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
       
        public List<string> GroupName { get; set; }
        public List<string> SmartTitle { get; set; }
        public List<string> SqlViewName { get; set; }
        public bool? isFunction { get; set; }
    }
}
