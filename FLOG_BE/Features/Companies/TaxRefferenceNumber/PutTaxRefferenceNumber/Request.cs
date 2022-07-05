using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.TaxRefferenceNumber.PutTaxRefferenceNumber
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestTaxRefferenceUpdate Body { get; set; }
    }

    public class RequestTaxRefferenceUpdate
    { 
        public string TaxRefferenceId { get; set; }
        public DateTime StartDate { get; set; }
        public int ReffNoStart { get; set; }
        public int ReffNoEnd { get; set; }
        public int DocLength { get; set; }
        public int LastNo { get; set; }
        public int Status { get; set; }
    }
}
