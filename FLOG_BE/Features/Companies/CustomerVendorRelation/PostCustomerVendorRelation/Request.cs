using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.CustomerVendorRelation.PostCustomerVendorRelation
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestVendorBody Body { get; set; }
    }

    public class RequestVendorBody
    {
        public Guid CustomerId { get; set; }
        public Guid VendorId { get; set; }
  
    }
}
