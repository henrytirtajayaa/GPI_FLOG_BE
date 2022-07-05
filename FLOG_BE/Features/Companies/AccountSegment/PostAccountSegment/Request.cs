using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.AccountSegment.PostAccountSegment
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public List<RequestBodyAS> Body { get; set; }
    }

    public class RequestBodyAS
    {
        public string SegmentId { get; set; }
        public int SegmentNo { get; set; }
        public string Description { get; set; }
        public int Length { get; set; }
        public bool IsMainAccount { get; set; }

    }
}
