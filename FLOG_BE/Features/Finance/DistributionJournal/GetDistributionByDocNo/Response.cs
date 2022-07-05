using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.DistributionJournal.GetDistributionByDocNo
{
    public class Response
    {
        public List<ResponseHeader> DistributionJournal { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseHeader
    {
        public Guid DistributionHeaderId { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DocumentNo { get; set; }
        public string CurrencyCode { get; set; }
        public int DecimalPoint { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Description { get; set; }
        public decimal OriginatingTotal { get; set; }
        public decimal FunctionalTotal { get; set; }
        public int Status { get; set; } 
        public List<ResponseDetail> DistributionDetails { get; set; }
    }

    public class ResponseDetail
    {
        public Guid DistributionDetailId { get; set; }
        public string AccountId { get; set; }
        public string AccountDesc { get; set; }
        public string JournalNote { get; set; }
        public decimal OriginatingDebit { get; set; }
        public decimal OriginatingCredit { get; set; }
        public decimal FunctionalDebit { get; set; }
        public decimal FunctionalCredit { get; set; }
        public int Status { get; set; }


    }
}
