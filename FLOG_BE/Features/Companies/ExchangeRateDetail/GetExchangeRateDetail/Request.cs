using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ExchangeRateDetail.GetExchangeRateDetail
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public Guid ExchangeRateHeaderId { get; set; }
        public List<DateTime?> RateDateStart { get; set; }
        public List<DateTime?> RateDateEnd { get; set; }
        public List<DateTime?> ExpiredDateStart { get; set; }
        public List<DateTime?> ExpiredDateEnd { get; set; }
        public List<decimal?> RateAmountMin { get; set; }
        public List<decimal?> RateAmountMax { get; set; }
        public List<int?> StatusMin { get; set; }
        public List<int?> StatusMax { get; set; }
    }
}
