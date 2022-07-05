using System;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Authentication.GetTokenDetail
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string Route { get; set; }
    }
}
