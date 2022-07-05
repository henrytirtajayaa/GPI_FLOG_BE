using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.Account.GetAccount
{
    public class Response
    {
        public List<ResponseItem> Accounts { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
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
        public string PostingType { get; set; }
        public string NormalBalance { get; set; }
        public string NoDirectEntry { get; set; }
        public bool Revaluation { get; set; }
        public string Inactive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
