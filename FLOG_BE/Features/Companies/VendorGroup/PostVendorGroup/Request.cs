using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.VendorGroup.PostVendorGroup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyPostVendorGroup Body { get; set; }
    }

    public class RequestBodyPostVendorGroup
    {
        public string VendorGroupCode { get; set; }
        public string VendorGroupName { get; set; }
        public string PaymentTermCode { get; set; }
        public string PayableAccountNo { get; set; }
        public string AccruedPayableAccountNo { get; set; }
        public bool InActive { get; set; }
    }
}
