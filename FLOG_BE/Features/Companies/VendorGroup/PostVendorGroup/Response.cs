using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Companies.VendorGroup.PostVendorGroup
{
    public class Response
    {
        public string VendorGroupId { get; set; }
        public string VendorGroupCode { get; set; }
        public string VendorGroupName { get; set; }
        public string PaymentTermCode { get; set; }
        public string PayableAccountNo { get; set; }
        public string AccruedPayableAccountNo { get; set; }
        public bool InActive { get; set; }
    }
}
