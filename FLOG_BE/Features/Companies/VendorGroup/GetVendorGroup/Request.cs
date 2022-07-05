using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.VendorGroup.GetVendorGroup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public List<string> VendorGroupCode { get; set; }
        public List<string> VendorGroupName { get; set; }
        public List<string> PaymentTermCode { get; set; }
        public List<string> PayableAccountNo { get; set; }
        public List<string> AccruedPayableAccountNo { get; set; }
        public bool? InActive { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<DateTime?> ModifiedDate { get; set; }
    }
}
