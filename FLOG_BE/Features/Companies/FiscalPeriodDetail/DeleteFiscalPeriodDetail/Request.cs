using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.FiscalPeriodDetail.DeleteFiscalPeriodDetail
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestFiscalDetailDelete Body { get; set; }
    }

    public class RequestFiscalDetailDelete
    {
        public Guid FiscalHeaderId { get; set; }
    }
}
