using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.Currency.PutCurrency
{
    public class Response
    {
        public Guid CurrencyId { get; set; }
        public string CurrencyCode { get; set; }

    }


}
