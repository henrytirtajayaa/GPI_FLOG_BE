using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.NumberFormatDetail.GetNumberFormatDetail
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
        public List<string> FormatHeaderId { get; set; }
        public List<int?> SegmentNoMin { get; set; }
        public List<int?> SegmentNoMax { get; set; }
        public List<int?> SegmentTypeMin { get; set; }
        public List<int?> SegmentTypeMax { get; set; }
        public List<int?> SegmentLengthMin { get; set; }
        public List<int?> SegmentLengthMax { get; set; }
        public List<string> MaskFormat { get; set; }
        public List<int?> StartingValueMin { get; set; }
        public List<int?> StartingValueMax { get; set; }
        public List<int?> EndingValueMin { get; set; }
        public List<int?> EndingValueMax { get; set; }
        public bool? Increase { get; set; }
       

    }
}
