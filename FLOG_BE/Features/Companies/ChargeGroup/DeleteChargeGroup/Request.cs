using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ChargeGroup.DeleteChargeGroup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestChargeGroupDelete Body { get; set; }
    }

    public class RequestChargeGroupDelete
    {
        public String ChargeGroupId { get; set; }
    }
}
