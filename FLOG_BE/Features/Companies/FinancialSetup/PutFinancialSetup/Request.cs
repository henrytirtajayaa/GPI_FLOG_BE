using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.FinancialSetup.PutFinancialSetup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyUpdateFinancialSetup Body { get; set; }
    }

    public class RequestBodyUpdateFinancialSetup
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
