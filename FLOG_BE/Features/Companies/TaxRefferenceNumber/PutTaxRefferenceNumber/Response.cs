using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Companies.TaxRefferenceNumber.PutTaxRefferenceNumber
{
    public class Response
    {
        public DateTime StartDate { get; set; }
        public int ReffNoStart { get; set; }
        public int ReffNoEnd { get; set; }
        public int DocLength { get; set; }
        public int LastNo { get; set; }
        public int Status { get; set; }
    }
}
