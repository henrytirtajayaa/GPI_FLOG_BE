using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;

namespace FLOG_BE.Features.Central.SecurityRoles.PostSecurityRole
{
    public class Response
    {
        public string RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
    }
    public class ResponseRoleAccess
    {

        public string SecurityRoleAccessId { get; set; }
        public string SecurityRoleId { get; set; }
        public string FormId { get; set; }
        public bool AllowAccessNew { get; set; }
        public bool AllowAccessOpen { get; set; }
        public bool AllowAccessEdit { get; set; }
        public bool AllowAccessDelete { get; set; }
        public bool AllowAccessPost { get; set; }
        public bool AllowAccessPrint { get; set; }
    }
}
