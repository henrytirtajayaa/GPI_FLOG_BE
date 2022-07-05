using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.NumberFormatDetail.PostNumberFormatDetail
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyPostNumberFormatDetail Body { get; set; }
    }

    public class RequestBodyPostNumberFormatDetail
    {
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
