using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Currency.PutCurrency
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutCurrencyBody Body { get; set; }
    }

    public class RequestPutCurrencyBody
    {
        public Guid CurrencyId { get; set; }
        public string Description { get; set; }
        public string Symbol { get; set; }
        public int DecimalPlaces { get; set; }
        public string RealizedGainAcc { get; set; }
        public string RealizedLossAcc { get; set; }
        public string UnrealizedGainAcc { get; set; }
        public string UnrealizedLossAcc { get; set; }
        public bool Inactive { get; set; }
        public string CurrencyUnit { get; set; }
        public string CurrencySubUnit { get; set; }

    }
}
