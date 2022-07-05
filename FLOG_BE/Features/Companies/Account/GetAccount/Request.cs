using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Account.GetAccount
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
        public List<string> AccountId { get; set; }
        public List<string> Segment1 { get; set; }
        public List<string> Segment2 { get; set; }
        public List<string> Segment3 { get; set; }
        public List<string> Segment4 { get; set; }
        public List<string> Segment5 { get; set; }
        public List<string> Segment6 { get; set; }
        public List<string> Segment7 { get; set; }
        public List<string> Segment8 { get; set; }
        public List<string> Segment9 { get; set; }
        public List<string> Segment10 { get; set; }
        public List<string> Description { get; set; }
        public bool? PostingType { get; set; }
        public bool? NormalBalance { get; set; }
        public bool? NoDirectEntry { get; set; }
        public bool? Revaluation { get; set; }
        public bool? Inactive { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
    }
}
