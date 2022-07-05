using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Container.DeleteContainer
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyDeleteContainer Body { get; set; }
    }

    public class RequestBodyDeleteContainer
    {
        public Guid ContainerId { get; set; }
    }
}
