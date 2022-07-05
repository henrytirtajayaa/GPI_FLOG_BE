﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.ARApply.DeleteApplyReceivable
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestDeleteBody Body { get; set; }
    }

    public class RequestDeleteBody
    {
        public Guid ReceivableApplyId { get; set; }

    }

}
