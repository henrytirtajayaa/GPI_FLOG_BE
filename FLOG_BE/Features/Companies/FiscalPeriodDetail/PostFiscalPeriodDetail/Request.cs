using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.FiscalPeriodDetail.PostFiscalPeriodDetail
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public List<RequestFiscalDetailBody> Body { get; set; }
    }

    public class RequestFiscalDetailBody
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
