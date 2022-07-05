using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.ApprovalSetupDetail.GetApprovalSetupDetail
{
    public class Response
    {
        public List<ResponseItem> ApprovalSetupDetail { get; set; }
        public ListInfo ListInfo { get; set; }
        public int CoaTotalLength { get; set;}
    }

    public class ResponseItem
    {
        public Guid ApprovalSetupDetailId { get; set; }
        public Guid ApprovalSetupHeaderId { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? PersonCategoryId { get; set; }
        public string PersonName { get; set; }
        public string Id { get; set; }        
        public string Description { get; set; }
        public int Level { get; set; }
        public bool? HasLimit { get; set; }
        public decimal ApprovalLimit { get; set; }
        public int Status { get; set; }

    }
}
