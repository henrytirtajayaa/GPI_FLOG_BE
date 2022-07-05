using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Vendor.DeleteVendor
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestVendorDelete Body { get; set; }
    }

    public class RequestVendorDelete
    {
        public Guid VendorId { get; set; }
    }
}
