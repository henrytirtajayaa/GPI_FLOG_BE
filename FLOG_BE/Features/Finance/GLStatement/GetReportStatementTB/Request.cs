using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.GLStatement.GetReportStatementTB
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }

        public RequestFilter Filter { get; set; }
    }

    public class RequestFilter
    {
        public int PeriodIndex{ get; set; }
        public int PeriodYear { get; set; }
        public string BranchCode { get; set; }
        public bool ShowZeroValue { get; set; }
    }
}
