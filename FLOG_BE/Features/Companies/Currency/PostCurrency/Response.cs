using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.Currency.PostCurrency
{
    public class Response
    {

        public Guid CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public string Symbol { get; set; }
        public int DecimalPlaces { get; set; }
        public string CurrencyUnit { get; set; }
        public string CurrencySubUnit { get; set; }
        public string RealizedGainAcc { get; set; }
        public string RealizedLossAcc { get; set; }
        public string UnrealizedGainAcc { get; set; }
        public string UnrealizedLossAcc { get; set; }
        public bool Inactive { get; set; }

    }


}
