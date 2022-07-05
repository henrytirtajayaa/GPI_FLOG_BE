using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.CustomerVendorRelation.PutCustomerVendorRelation
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutBody Body { get; set; }
    }

    public class RequestPutBody
    {
        public Guid RelationId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid VendorId { get; set; }

    }
}
