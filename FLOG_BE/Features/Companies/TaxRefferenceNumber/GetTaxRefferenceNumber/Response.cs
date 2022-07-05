using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Utils;
using FLOG_BE.Model.Central.Entities;

namespace FLOG_BE.Features.Companies.TaxRefferenceNumber.GetTaxRefferenceNumber
{
    public class Response
    {
        public List<ResponseItem> TaxRefferenceNumbers { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    { 
        public string TaxRefferenceId { get; set; }
        public DateTime StartDate { get; set; }
        public int ReffNoStart { get; set; }
        public int ReffNoEnd { get; set; }
        public int DocLength { get; set; }
        public int LastNo { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
