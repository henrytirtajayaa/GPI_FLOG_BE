using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Sales.NegotiationSheet.GetNegotiationSheetApprovalComment
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
        public Guid NegotiationSheetId { get; set; }
        public Guid PersonId { get; set; }
        public int Index { get; set; }

    }
}
