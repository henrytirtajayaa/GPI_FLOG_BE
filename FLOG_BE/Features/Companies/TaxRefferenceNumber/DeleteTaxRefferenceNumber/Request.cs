using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.TaxRefferenceNumber.DeleteTaxRefferenceNumber
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestTaxRefferenceDelete Body { get; set; }
    }

    public class RequestTaxRefferenceDelete
    {
        public DateTime StartDate { get; set; }
        public string TaxRefferenceId { get; set; }
    }
}
