using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.SecurityRoles.PostSecurityRole
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestRole Body { get; set; }
       
    }

    public class RequestRole
    {
        public string RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public List<RequestRoleAccess> RoleAccess { get; set; }
        public List<RequestSmartRole> RoleSmart { get; set; }

    }
    public class RequestRoleAccess
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
    public class RequestSmartRole
    {
        public string Id { get; set; }
        public string SmartviewId { get; set; }
        public string SecurityRoleId { get; set; }
        
    }
}
