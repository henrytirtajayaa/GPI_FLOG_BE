using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.ExchangeRateHeader.GetExchangeRateHeader
{
    public class Response
    {
        public List<ResponseItem> ExchangeRateHeaders { get; set; }
        public ListInfo ListInfo { get; set; }
        public int CoaTotalLength { get; set;}
    }

    public class ResponseItem
    {
        public Guid ExchangeRateHeaderId { get; set; }
        public string ExchangeRateCode { get; set; }
        public string Description { get; set; }
        public string CurrencyCode { get; set; }
        public int RateType { get; set; }
        public string ExpiredPeriod { get; set; }
        public int CalculationType { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
