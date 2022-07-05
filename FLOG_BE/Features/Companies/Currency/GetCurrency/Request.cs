using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Currency.GetCurrency
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
        public List<string> CurrencyCode { get; set; }
        public List<string> Description { get; set; }
        public List<string> Symbol { get; set; }
        public List<int?> DecimalPlacesMin { get; set; }
        public List<int?> DecimalPlacesMax { get; set; }
        public List<string> RealizedGainAcc { get; set; }
        public List<string> RealizedLossAcc { get; set; }
        public List<string> UnrealizedGainAcc { get; set; }
        public List<string> UnrealizedLossAcc { get; set; }
        public bool? Inactive { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
        public List<string> CurrencyUnit { get; set; }
        public List<string> CurrencySubUnit { get; set; }
    }
}
