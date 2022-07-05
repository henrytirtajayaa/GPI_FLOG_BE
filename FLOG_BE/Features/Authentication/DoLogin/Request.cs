using System;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Authentication.DoLogin
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
