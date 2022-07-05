using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Companies.FinancialSetup.PutFinancialSetup
{
    public class Response
    {
        public string FuncCurrencyCode { get; set; }
        public int DefaultRateType { get; set; }
        public int TaxRateType { get; set; }
        public string UomScheduleCode { get; set; }
        public string CheckbookChargesType { get; set; }
        public int DeptSegmentNo { get; set; }
        public int Status { get; set; }
    }
}
