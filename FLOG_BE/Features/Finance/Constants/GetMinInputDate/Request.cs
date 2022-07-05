using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Constants.GetMinInputDate
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestFilter Filter { get; set; }
    }

    public class RequestFilter
    {
        public int TrxModule { get; set; }
    }
}
