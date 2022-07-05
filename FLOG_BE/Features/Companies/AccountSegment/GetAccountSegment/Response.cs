using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.AccountSegment.GetAccountSegment
{
    public class Response
    {
        public List<ResponseItem> AccountSegments { get; set; }
        public ListInfo ListInfo { get; set; }
        public int CoaTotalLength { get; set;}
    }

    public class ResponseItem
    {
        public string SegmentId { get; set; }
        public int SegmentNo { get; set; }
        public string Description { get; set; }
        public int Length { get; set; }
        public bool IsMainAccount { get; set; }
    }
}
