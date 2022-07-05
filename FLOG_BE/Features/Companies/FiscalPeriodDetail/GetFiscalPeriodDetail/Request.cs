using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.FiscalPeriodDetail.GetFiscalPeriodDetail
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
        public List<Guid?> FiscalHeaderId { get; set; }
        public List<int?> PeriodIndex { get; set; }
        public List<DateTime?> PeriodStart { get; set; }
        public List<DateTime?> PeriodEnd { get; set; }
        public bool? IsClosePurchasing { get; set; }
        public bool? IsCloseSales { get; set; }
        public bool? IsCloseInventory { get; set; }
        public bool? IsCloseFinancial { get; set; }
        public bool? IsCloseAsset { get; set; }
    }
}
