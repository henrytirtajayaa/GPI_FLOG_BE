using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.UserGroup.PostUserGroup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyPostUserGroup Body { get; set; }
    }

    public class RequestBodyPostUserGroup
    {
        public string UserGroupCode { get; set; }
        public string UserGroupName { get; set; }
    }
}
