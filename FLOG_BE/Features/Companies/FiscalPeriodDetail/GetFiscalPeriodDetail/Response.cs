using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.FiscalPeriodDetail.GetFiscalPeriodDetail
{
    public class Response
    {
        public List<ResponseItem> FiscalPeriodDetails { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid FiscalDetailId { get; set; }
        public Guid FiscalHeaderId { get; set; }
        public int PeriodIndex { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public bool IsClosePurchasing { get; set; }
        public bool IsCloseSales { get; set; }
        public bool IsCloseInventory { get; set; }
        public bool IsCloseFinancial { get; set; }
        public bool IsCloseAsset { get; set; }
    }
}
