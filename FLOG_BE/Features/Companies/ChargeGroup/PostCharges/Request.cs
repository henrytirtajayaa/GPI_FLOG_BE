using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ChargeGroup.PostChargeGroup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestChargeGroupBody Body { get; set; }
    }

    public class RequestChargeGroupBody
    {
        public string ChargeGroupId { get; set; }
        public string ChargeGroupCode { get; set; }
        public string ChargeGroupName { get; set; }
      

    }
}
