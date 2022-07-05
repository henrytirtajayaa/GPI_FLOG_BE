using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ExchangeRateHeader.GetCurrentExchangeRate
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestFilter Filter { get; set; }
    }

    public class RequestFilter
    {
        public string CurrencyCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public int RateType { get; set; }
    }
}
