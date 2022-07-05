using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.AccountSegment.GetAccountSegment
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
        public List<int> SegmentNo { get; set; }
        public List<string> Description { get; set; }
        public List<int> Length { get; set; }
        public bool? IsMainAccount { get; set; }
    }
}
