using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.GLStatement.GetGLStatementPeriod
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        //public RequestFilter Filter { get; set; }
    }

}
