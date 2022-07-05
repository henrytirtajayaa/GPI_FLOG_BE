using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.PayableSetup.GetPayableSetup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public List<int> DefaultRateType { get; set; }
        public List<int> TaxRateType { get; set; }
        public List<bool> AgingByDocdate { get; set; }
        public List<decimal> WriteoffLimit { get; set; }
    }
}
