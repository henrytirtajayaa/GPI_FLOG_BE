using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Report.Dashboard.GetMyApprovalList
{
    public class Response
    {
        public List<ResponseTask> MyApprovals { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseTask
    {
        public string IconClasses { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TaskCount { get; set; }
        public string TaskUrl { get; set; }
    }
}
