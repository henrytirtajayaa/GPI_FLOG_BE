using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.User.DeleteUser
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public RequestFilter Body { get; set; }
    }

    public class RequestFilter
    {
        public string UserId { get; set; }
    }
}
