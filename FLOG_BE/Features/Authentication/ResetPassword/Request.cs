using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Authentication.ResetPassword
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestData Body { get; set; }
    }

    public class RequestData
    {
        public string Email { get; set; }

    }
}
