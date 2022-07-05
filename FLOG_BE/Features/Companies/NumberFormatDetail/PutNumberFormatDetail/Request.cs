using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.NumberFormatDetail.PutNumberFormatDetail
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyUpdateNumberFormatDetail Body { get; set; }
    }

    public class RequestBodyUpdateNumberFormatDetail
    {
        public string FormatDetailId { get; set; }
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
