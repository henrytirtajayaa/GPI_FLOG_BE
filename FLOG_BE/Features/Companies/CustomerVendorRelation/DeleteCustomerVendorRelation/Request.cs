using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.CustomerVendorRelation.DeleteCustomerVendorRelation
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestDelete Body { get; set; }
    }

    public class RequestDelete
    {
        public Guid RelationId { get; set; }
    }
}
