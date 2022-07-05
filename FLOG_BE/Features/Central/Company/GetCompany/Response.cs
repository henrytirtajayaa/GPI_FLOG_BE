using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Central.Company.GetCompany
{
    public class Response
    {
        public List<ReponseItem> Companies { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ReponseItem
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string DatabaseId { get; set; }
        public string DatabaseAddress { get; set; }
        public string DatabasePassword { get; set; }
        public string CoaSymbol { get; set; }
        public int CoaTotalLength { get; set; }
        public bool InActive { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }

    }
}
