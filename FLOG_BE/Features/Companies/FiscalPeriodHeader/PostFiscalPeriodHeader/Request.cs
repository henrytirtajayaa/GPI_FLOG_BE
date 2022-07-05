using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.FiscalPeriodHeader.PostFiscalPeriodHeader
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestFiscalBody Body { get; set; }
    }

    public class RequestFiscalBody
    {
        public int PeriodYear { get; set; }
        public int TotalPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool ClosingYear { get; set; }

        public List<RequestFiscalDetailBody> FiscalDetails { get; set; }

    }

    public class RequestFiscalDetailBody
    {
        public Guid FiscalDetailId { get; set; }
        public string FiscalHeaderId { get; set; }
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
