using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.UserGroup.PutUserGroup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyUpdateUserGroup Body { get; set; }
    }

    public class RequestBodyUpdateUserGroup
    {
        public string UserGroupId { get; set; }
        public string UserGroupName { get; set; }
    }
}
