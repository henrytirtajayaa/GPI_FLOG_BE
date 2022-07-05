using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class NumberFormatDetail
    {
        public NumberFormatDetail()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid FormatDetailId { get; set; }
        public Guid FormatHeaderId { get; set; }
        public int SegmentNo { get; set; }
        public int SegmentType { get; set; }
        public int SegmentLength { get; set; }
        public string MaskFormat { get; set; }
        public int StartingValue { get; set; }
        public int EndingValue { get; set; }
        public bool Increase { get; set; }
        #endregion

        #region Generated Relationships
        public NumberFormatHeader NumberFormatHeader { get; set; }
        #endregion
    }
}
