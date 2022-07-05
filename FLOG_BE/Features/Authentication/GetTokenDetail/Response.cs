using FLOG_BE.Helper.dto;
using FLOG_BE.Model.Central.Entities;
using System.Collections.Generic;

namespace FLOG_BE.Features.Authentication.GetTokenDetail
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string ErrorDesc { get; set; }
        public ResponsePerson DetailUser { get; set; }
        public ResponseCompany Company { get; set; }
      
        public ResponseRole Role { get; set; }
        public List<UserRole> ListRole { get; set; }
        public List<MenuItem> Menus { get; set; }
        public string AccessToken { get; set; }
        public ResponseRoleAccess RoleAccess { get; set; }
        public string CompanySecurityId { get; set; }
        public string SessionId { get; set; }
    }
    public class UserRole
    {
        public string SecurityId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string RoleName { get; set; }
       

    }
    public class ResponsePerson
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }

        public string EmailAddress { get; set; }

        public string UserGroupId { get; set; }
        public string UserGroupName { get; set; }
       
       
    }

    public class ResponseCompany
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ResponseRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
       
    }
    public class ResponseRoleAccess
    {
        public string SecurityRoleId { get; set; }
        public bool AllowNew { get; set; }
        public bool AllowOpen { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowPost { get; set; }
        public bool AllowPrint { get; set; }
    }


}
