using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FLOG_BE.Features.Finance.Payable.PutPayable;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Payable.PutStatusPayable
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPayable Body { get; set; }
    }
}
