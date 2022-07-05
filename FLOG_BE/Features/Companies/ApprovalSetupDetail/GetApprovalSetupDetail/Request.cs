using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ApprovalSetupDetail.GetApprovalSetupDetail
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
        public Guid ApprovalSetupHeaderId { get; set; }

        public List<string> Description { get; set; }
        public List<int> Level { get; set; }
        public bool? HasLimit { get; set; }
        public List<decimal?> ApprovalLimit { get; set; }
        public List<int?> Status { get; set; }
    }
}
