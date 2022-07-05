using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FLOG_BE.Features.Companies.FiscalPeriodHeader.PostFiscalPeriodHeader;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.FiscalPeriodHeader.PutFiscalPeriodHeader
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutFiscalBody Body { get; set; }
    }

    public class RequestPutFiscalBody
    {
        public Guid FiscalHeaderId { get; set; }
        public int PeriodYear { get; set; }
        public int TotalPeriod { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool ClosingYear { get; set; }

        public List<RequestFiscalDetailBody> FiscalDetails { get; set; }
    }
}
