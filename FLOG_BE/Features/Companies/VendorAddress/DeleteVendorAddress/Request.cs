using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.VendorAddress.DeleteVendorAddress
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyDeleteVendorAddress Body { get; set; }
    }

    public class RequestBodyDeleteVendorAddress
    {
        public Guid VendorAddressId { get; set; }
    }
}
