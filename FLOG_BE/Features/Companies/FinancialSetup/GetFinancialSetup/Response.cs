using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOG_BE.Features.Companies.FinancialSetup.GetFinancialSetup
{
    public class Response
    {
        public List<ResponseItem> FinancialSetups { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public string FinancialSetupId { get; set; }
        public string FuncCurrencyCode { get; set; }
        public int DefaultRateType { get; set; }
        public int TaxRateType { get; set; }
        public string UomScheduleCode { get; set; }
        public int DeptSegmentNo { get; set; }
        public string CheckbookChargesType { get; set; }
        public int Status { get; set; }
    }
}
