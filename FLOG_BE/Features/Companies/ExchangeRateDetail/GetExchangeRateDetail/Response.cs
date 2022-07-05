using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.ExchangeRateDetail.GetExchangeRateDetail
{
    public class Response
    {
        public List<ResponseItem> GetExchangeRateDetail { get; set; }
        public ListInfo ListInfo { get; set; }
        public int CoaTotalLength { get; set; }
    }

    public class ResponseItem
    {
        public Guid ExchangeRateDetailId { get; set; }
        public Guid ExchangeRateHeaderId { get; set; }
        public DateTime RateDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public decimal RateAmount { get; set; }
        public int Status { get; set; }

    }
}
