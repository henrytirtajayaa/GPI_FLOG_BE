using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;
using System;

namespace FLOG_BE.Features.Companies.Currency.GetCurrency
{
    public class Response
    {
        public List<ResponseItem> Currencies { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public string Symbol { get; set; }
        public int DecimalPlaces { get; set; }
        public string RealizedGainAcc { get; set; }
        public string RealizedLossAcc { get; set; }
        public string UnrealizedGainAcc { get; set; }
        public string UnrealizedLossAcc { get; set; }
        public bool IsFunctional { get; set; }
        public bool Inactive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CurrencyUnit { get; set; }
        public string CurrencySubUnit { get; set; }
    }
}
