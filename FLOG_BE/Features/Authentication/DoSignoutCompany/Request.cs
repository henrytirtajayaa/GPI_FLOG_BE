using System;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Authentication.DoSignoutCompany
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }

        public string PersonId { get; set; }
        public string CompanySecurityId { get; set; }
        public string CompanyId { get; set; }
    }
}
