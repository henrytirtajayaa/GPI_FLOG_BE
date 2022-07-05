using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ExchangeRateHeader.PostExchangeRateHeader
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestExchangeRateHeader Body { get; set; }

    }

    public class RequestExchangeRateHeader
    {
        public string ExchangeRateCode { get; set; }
        public string Description { get; set; }
        public string CurrencyCode { get; set; }
        public int RateType { get; set; }
        public string ExpiredPeriod { get; set; }
        public int CalculationType { get; set; }
        public int Status { get; set; }
        public List<ExchangeRateDetails> ExchangeRateDetails { get; set; }

    }
    public class ExchangeRateDetails
    {
        public DateTime RateDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public decimal RateAmount { get; set; }
        public int Status { get; set; }
    }
}
