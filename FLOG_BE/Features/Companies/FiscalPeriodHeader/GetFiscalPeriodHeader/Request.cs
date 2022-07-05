using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.FiscalPeriodHeader.GetFiscalPeriodHeader
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public List<int?> PeriodYearMin { get; set; }
        public List<int?> PeriodYearMax { get; set; }
        public List<int?> TotalPeriodMin { get; set; }
        public List<int?> TotalPeriodMax { get; set; }
        public List<DateTime?> StartDateStart { get; set; }
        public List<DateTime?> StartDateEnd { get; set; }
        public List<DateTime?> EndDateStart { get; set; }
        public List<DateTime?> EndDateEnd { get; set; }
        public bool? ClosingYear { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<DateTime?> ModifiedDate { get; set; }
    }
}
