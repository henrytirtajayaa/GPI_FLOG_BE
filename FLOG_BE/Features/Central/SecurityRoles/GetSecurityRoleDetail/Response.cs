using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;

namespace FLOG_BE.Features.Central.SecurityRoles.GetSecurityRoleDetail
{
    public class Response
    {
        public List<ReponseItem> SecurityRoles { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ReponseItem
    {
        public string RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public List<ResponseRoleAccess> RoleAccess { get; set; }


    }
    public class ResponseRoleAccess
    {
        public string SecurityRoleAccessId { get; set; }
        public string SecurityRoleId { get; set; }
        public string FormId { get; set; }
        public string Name { get; set; }
        public bool AllowAccessNew { get; set; }
        public bool AllowAccessOpen { get; set; }
        public bool AllowAccessEdit { get; set; }
        public bool AllowAccessDelete { get; set; }
        public bool AllowAccessPost { get; set; }
        public bool AllowAccessPrint { get; set; }
    }
}
