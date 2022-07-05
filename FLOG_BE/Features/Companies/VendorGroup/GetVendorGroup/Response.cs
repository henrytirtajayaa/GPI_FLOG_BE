using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Companies.VendorGroup.GetVendorGroup
{
    public class Response
    {
        public List<ResponseItem> VendorGroups { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public string VendorGroupId { get; set; }
        public string VendorGroupCode { get; set; }
        public string VendorGroupName { get; set; }
        public string PaymentTermCode { get; set; }
        public string PayableAccountNo { get; set; }
        public string AccruedPayableAccountNo { get; set; }
        public bool InActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
