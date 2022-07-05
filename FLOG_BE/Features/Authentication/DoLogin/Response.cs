using FLOG_BE.Model.Central.Entities;
using System.Collections.Generic;

namespace FLOG_BE.Features.Authentication.DoLogin
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string ErrorDesc { get; set; }
        public DetailUser DetailUser { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public string AccessToken { get; set; }
    }

    public class DetailUser
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }

        public string EmailAddress { get; set; }

        public string UserGroupId { get; set; }
    }

    public class UserRole
    {
        public string SecurityId { get; set; }
        public string CompanyName { get; set; }
        public string RoleName { get; set; }

    }
}
