using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Central.Smartview.GetSmartview
{
    public class Response
    {
        public List<ReponseItem> Smartviews { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ReponseItem
    {
        public Guid SmartviewId { get; set; }
        public string GroupName { get; set; }
        public string SmartTitle { get; set; }
        public string SqlViewName { get; set; }
        public bool isFunction { get; set; }

    }
}
