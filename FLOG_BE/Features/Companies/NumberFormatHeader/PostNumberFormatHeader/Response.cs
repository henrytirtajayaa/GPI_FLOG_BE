using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Companies.NumberFormatHeader.PostNumberFormatHeader
{
    public class Response
    {
        public string FormatHeaderId { get; set; }
        public string DocumentId { get; set; }
        public string Description { get; set; }
        public string LastGeneratedNo { get; set; }
        public string NumberFormat { get; set; }
        public bool InActive { get; set; }
        public bool IsMonthlyReset { get; set; }
        public bool IsYearlyReset { get; set; }
    }
}
