using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.FiscalPeriodHeader.PutFiscalPeriodHeader
{
    public class Response
    {
        public Guid FiscalHeaderId { get; set; }
        public int PeriodYear { get; set; }

    }


}
