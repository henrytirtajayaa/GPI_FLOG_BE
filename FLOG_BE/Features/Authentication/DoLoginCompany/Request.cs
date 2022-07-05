using System;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Authentication.DoLoginCompany
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string CompanyId { get; set; }
    }
}
