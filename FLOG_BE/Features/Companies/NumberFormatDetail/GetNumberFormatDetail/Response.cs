using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Companies.NumberFormatDetail.GetNumberFormatDetail
{
    public class Response
    {
        public List<ResponseItem> NumberFormatDetails { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public string FormatDetailId { get; set; }
        public string FormatHeaderId { get; set; }
        public int SegmentNo { get; set; }
        public int SegmentType { get; set; }
        public int SegmentLength { get; set; }
        public string MaskFormat { get; set; }
        public int StartingValue { get; set; }
        public int EndingValue { get; set; }
        public bool Increase { get; set; }
      
    }
}
