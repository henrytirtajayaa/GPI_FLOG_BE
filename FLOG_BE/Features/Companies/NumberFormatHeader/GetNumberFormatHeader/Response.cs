using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Companies.NumberFormatHeader.GetNumberFormatHeader
{
    public class Response
    {
        public List<ResponseItem> NumberFormatHeaders { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public string FormatHeaderId { get; set; }
        public string DocumentId { get; set; }
        public string Description { get; set; }
        public string LastGeneratedNo { get; set; }
        public string NumberFormat { get; set; }
        public bool InActive { get; set; }
        public bool IsMonthlyReset { get; set; }
        public bool IsYearlyReset { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
