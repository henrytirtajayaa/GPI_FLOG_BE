using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ShippingLine.PostShippingLine
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestShippingLineBody Body { get; set; }
    }

    public class RequestShippingLineBody
    {
        public string ShippingLineCode { get; set; }
        public string ShippingLineName { get; set; }
        public string ShippingLineType { get; set; }
        public Guid VendorId { get; set; }
        public bool IsFeeder { get; set; }
        public bool Inactive { get; set; }
        public int Status { get; set; }

    }
}
