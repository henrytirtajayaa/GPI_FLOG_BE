using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.CompanyBranch.GetCompanyBranch
{
    public class Response
    {
        public List<ResponseItem> CompanyBranches { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid CompanyBranchId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public bool Default { get; set; }
        public bool Inactive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
