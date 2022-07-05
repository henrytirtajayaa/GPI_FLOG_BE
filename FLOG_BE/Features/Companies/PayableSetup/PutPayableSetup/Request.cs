using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.PayableSetup.PutPayableSetup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyUpdatePayableSetup Body { get; set; }
    }

    public class RequestBodyUpdatePayableSetup
    {
        public string PayableSetupId { get; set; }
        public int DefaultRateType { get; set; }
        public int TaxRateType { get; set; }
        public bool AgingByDocdate { get; set; }
        public decimal WriteoffLimit { get; set; }
    }
}
