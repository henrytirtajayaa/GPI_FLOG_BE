using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Constants.GetDocNoSetup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
    }
}
