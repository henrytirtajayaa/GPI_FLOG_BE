using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Companies.ExchangeRateHeader.PutExchangeRateHeader
{
    public class Response
    {
        public Guid ExchangeRateHeaderId { get; set; }
        public string ExchangeRateCode { get; set; }
        public string Description { get; set; }
        public string CurrencyCode { get; set; }
        public int RateType { get; set; }
        public string ExpiredPeriod { get; set; }
        public int CalculationType { get; set; }
        public int Status { get; set; }
    }
}
