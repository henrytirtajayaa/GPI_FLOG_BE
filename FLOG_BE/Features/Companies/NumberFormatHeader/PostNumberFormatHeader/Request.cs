using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.NumberFormatHeader.PostNumberFormatHeader
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyPostNumberFormatHeader Body { get; set; }
    }

    public class RequestBodyPostNumberFormatHeader
    {
        
        public string DocumentId { get; set; }
        public string Description { get; set; }
        public string LastGeneratedNo { get; set; }
        public string NumberFormat { get; set; }
        public bool InActive { get; set; }
        public bool IsMonthlyReset { get; set; }
        public bool IsYearlyReset { get; set; }
        public List<RequestBodyPostNumberFormatDetail> NumberFormatDetails { get; set; }
    }

    public class RequestBodyPostNumberFormatDetail
    {
        public Guid FormatDetailId { get; set; }
        public string FormatHeaderId { get; set; }
        public int SegmentNo { get; set; }
        public int SegmentType { get; set; }
        public int SegmentLength { get; set; }
        public string MaskFormat { get; set; }
        public int StartingValue { get; set; }
        public int EndingValue { get; set; }
        public bool Increase { get; set; }
    }
}
