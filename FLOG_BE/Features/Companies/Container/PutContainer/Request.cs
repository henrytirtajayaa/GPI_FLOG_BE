using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Container.PutContainer
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyUpdateContainer Body { get; set; }
    }

    public class RequestBodyUpdateContainer
    {
        public Guid Containerid { get; set; }
        public string ContainerName { get; set; }
        public int ContainerSize { get; set; }
        public string ContainerType { get; set; }
        public bool IsReefer { get; set; }
        public int ContainerTeus { get; set; }
        public bool Inactive { get; set; }

    }
}
