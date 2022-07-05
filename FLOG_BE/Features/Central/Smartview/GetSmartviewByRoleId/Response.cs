using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Central.Smartview.GetSmartviewByRoleId
{
    public class Response
    {
        public List<ResponseItem> Smartviews { get; set; }
    }
    public class ResponseItem
    {
        public Guid SmartviewId { get; set; }
        public string GroupName { get; set; }
        public string SmartTitle { get; set; }
        public string SqlViewName { get; set; }
        public bool isFunction { get; set; }
    }
}
