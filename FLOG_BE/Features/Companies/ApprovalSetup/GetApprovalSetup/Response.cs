using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.ApprovalSetup.GetApprovalSetup
{
    public class Response
    {
        public List<ResponseItem> ApprovalSetup { get; set; }
        public ListInfo ListInfo { get; set; }
        public int CoaTotalLength { get; set;}
    }

    public class ResponseItem
    {
        public string ApprovalSetupHeaderId { get; set; }
        public string ApprovalCode { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
