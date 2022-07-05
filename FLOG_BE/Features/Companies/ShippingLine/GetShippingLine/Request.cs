using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ShippingLine.GetShippingLine
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public List<string> ShippingLineCode { get; set; }
        public List<string> ShippingLineName { get; set; }
        public List<string> ShippingLineType { get; set; }
        public List<Guid?> VendorId { get; set; }
        public bool? IsFeeder { get; set; }
        public bool? Inactive { get; set; }
        public List<int?> Status { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<DateTime?> ModifiedDate { get; set; }

    }
}
