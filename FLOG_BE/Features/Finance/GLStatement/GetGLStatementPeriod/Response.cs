using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.GLStatement.GetGLStatementPeriod
{
    public class Response
    {
        public List<int> ClosedYears { get; set; }
        public List<PeriodResponse> Periods { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class PeriodResponse
    {
        public int PeriodYear { get; set; }

        public List<int> PeriodIndexs { get; set; }
    }

}
