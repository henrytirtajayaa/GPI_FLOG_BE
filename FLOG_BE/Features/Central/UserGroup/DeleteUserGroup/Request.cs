using Infrastructure.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Central.UserGroup.DeleteUserGroup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set;  }

        public RequestBodyDeleteUserGroup Body { get; set; }
    }

    public class RequestBodyDeleteUserGroup
    { 
        public string UserGroupId { get; set; }
    }
}
