using FLOG_BE.Model.Central.Entities;
using FLOG_BE.Model.Companies.View;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.GLStatement.GetReportStatementTB
{
    public class Response
    {
        public ResponseReportTitle ReportTitle { get; set; }
        public List<TrialBalance> TrialBalances { get; set; }
        
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseReportTitle
    {
        public string CompanyName { get; set; }
        public string LogoImageUrl { get; set; }

        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string PeriodOf { get; set; }

        public int DecimalPlaces { get; set; }
    }


}
