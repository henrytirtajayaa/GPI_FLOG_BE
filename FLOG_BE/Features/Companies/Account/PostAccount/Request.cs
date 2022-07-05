using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Account.PostAccount
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestAccountBody Body { get; set; }
    }

    public class RequestAccountBody
    {
       
        public string AccountId { get; set; }
        public string Segment1 { get; set; }
        public string Segment2 { get; set; }
        public string Segment3 { get; set; }
        public string Segment4 { get; set; }
        public string Segment5 { get; set; }
        public string Segment6 { get; set; }
        public string Segment7 { get; set; }
        public string Segment8 { get; set; }
        public string Segment9 { get; set; }
        public string Segment10 { get; set; }
        public string Description { get; set; }
        public bool PostingType { get; set; }
        public bool NormalBalance { get; set; }
        public bool NoDirectEntry { get; set; }
        public bool Revaluation { get; set; }
        public bool Inactive { get; set; }

    }
}
