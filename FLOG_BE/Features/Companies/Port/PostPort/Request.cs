using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Port.PostPort
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPortBody Body { get; set; }
    }

    public class RequestPortBody
    {
        public string PortCode { get; set; }
        public string PortName { get; set; }
        public string PortType { get; set; }
        public string CityCode { get; set; }
        public bool InActive { get; set; }
    }
}
