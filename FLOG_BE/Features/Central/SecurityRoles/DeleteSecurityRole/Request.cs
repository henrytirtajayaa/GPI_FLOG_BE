using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.SecurityRoles.DeleteSecurityRole
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodySecurityRoleDelete Body { get; set; }
    }

    public class RequestBodySecurityRoleDelete
    {
        public string RoleId { get; set; }
    }
}
