using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.GLClosing.GetClosingPeriod
{
    public class Response
    {
        public PeriodResponse Period { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class PeriodResponse
    {
        public int PeriodYear { get; set; }

        public List<int> PeriodIndexs { get; set; }
    }

}
