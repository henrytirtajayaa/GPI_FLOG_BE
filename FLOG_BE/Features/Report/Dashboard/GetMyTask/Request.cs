using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Report.Dashboard.GetMyTask
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
    }
}
