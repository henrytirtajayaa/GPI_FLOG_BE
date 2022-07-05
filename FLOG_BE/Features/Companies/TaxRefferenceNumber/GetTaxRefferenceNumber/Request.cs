using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.TaxRefferenceNumber.GetTaxRefferenceNumber
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
        public List<DateTime?> StartDateStart { get; set; }
        public List<DateTime?> StartDateEnd { get; set; }
        public List<int?> ReffNoStartMin { get; set; }
        public List<int?> ReffNoStartMax { get; set; }
        public List<int?> ReffNoEndMin { get; set; }
        public List<int?> ReffNoEndMax { get; set; }
        public List<int?> DocLengthMin { get; set; }
        public List<int?> DocLengthMax { get; set; }
        public List<int?> LastNoMin { get; set; }
        public List<int?> LastNoMax { get; set; }
        public List<int?> Status { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
    }
}
