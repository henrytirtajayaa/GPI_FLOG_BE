using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.FiscalPeriodHeader.DeleteFiscalPeriodHeader
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestFiscalDelete Body { get; set; }
    }

    public class RequestFiscalDelete
    {
        public Guid FiscalHeaderId { get; set; }
    }
}
