using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Uom.GetUomHeader
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
        public List<string> UomScheduleCode { get; set; }
        public List<string> UomScheduleName { get; set; }
        public List<string> UomBaseCode { get; set; }
        public List<string> UomBaseName { get; set; }
    }
}
