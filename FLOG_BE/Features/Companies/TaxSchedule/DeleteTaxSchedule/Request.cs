using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.TaxSchedule.DeleteTaxSchedule
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestTaxScheduleDelete Body { get; set; }
    }

    public class RequestTaxScheduleDelete
    {
        public String TaxScheduleId { get; set; }
    }
}
