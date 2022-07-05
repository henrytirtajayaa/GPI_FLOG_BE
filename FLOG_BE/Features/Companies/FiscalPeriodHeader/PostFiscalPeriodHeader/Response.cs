using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.FiscalPeriodHeader.PostFiscalPeriodHeader
{
    public class Response
    {
        public Guid FiscalHeaderId { get; set; }
        public int PeriodYear { get; set; }
        public int TotalPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool ClosingYear { get; set; }

    }


}
