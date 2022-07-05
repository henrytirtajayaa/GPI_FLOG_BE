using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.VendorGroup.DeleteVendorGroup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyDeleteVendorGroup Body { get; set; }
    }

    public class RequestBodyDeleteVendorGroup
    {
        public string VendorGroupId { get; set; }
    }
}
