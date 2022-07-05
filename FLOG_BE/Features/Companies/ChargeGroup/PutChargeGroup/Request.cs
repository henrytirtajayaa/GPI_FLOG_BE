using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ChargeGroup.PutChargeGroup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutChargeBody Body { get; set; }
    }

    public class RequestPutChargeBody
    {
        public string ChargeGroupId { get; set; }
        public string ChargeGroupCode { get; set; }
        public string ChargeGroupName { get; set; }
      

    }
}
