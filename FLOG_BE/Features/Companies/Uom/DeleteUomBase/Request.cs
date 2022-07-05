﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Uom.DeleteUomBase
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestDelete Body { get; set; }
    }

    public class RequestDelete
    {
        public Guid UomBaseId { get; set; }
    }
}
