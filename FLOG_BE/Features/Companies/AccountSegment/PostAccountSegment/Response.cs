using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.AccountSegment.PostAccountSegment
{
    public class Response
    {
        public List<ReponseItem> AccountSegments { get; set; }
    }

    public class ReponseItem
    {
        public string SegmentId { get; set; }
        public int SegmentNo { get; set; }
        public string Description { get; set; }
        public int Length { get; set; }
        public bool IsMainAccount { get; set; }
    }
}
