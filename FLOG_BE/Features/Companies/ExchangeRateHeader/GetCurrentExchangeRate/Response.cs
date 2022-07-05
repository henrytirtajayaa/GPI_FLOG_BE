using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.ExchangeRateHeader.GetCurrentExchangeRate
{
    public class Response
    {
        public decimal ExcRate { get; set;}
        public bool IsMultiply { get; set; }
    }

}
