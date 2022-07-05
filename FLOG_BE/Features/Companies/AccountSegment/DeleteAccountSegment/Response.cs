using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Companies.AccountSegment.DeleteAccountSegment
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
