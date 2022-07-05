using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.User.PostUser
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyUser Body { get; set; }
    }

    public class RequestBodyUser
    {
        public string UserFullName { get; set; }
        public string UserPassword { get; set; }
        public string EmailAddress { get; set; }
        public string UserGroupId { get; set; }
        public bool InActive { get; set; }
    }
}
