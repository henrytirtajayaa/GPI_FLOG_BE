using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Central.CompanySecurity.GetCompanySecurity
{
    public class Response
    {
        public List<ReponseItem> CompanySecurities { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ReponseItem
    {
        public string CompanySecurityId { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string RoleId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string CompanyName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
