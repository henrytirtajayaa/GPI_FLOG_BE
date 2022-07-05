using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Companies.Port.PostPort
{
    public class Response
    {
        public string PortId { get; set; }
        public string PortCode { get; set; }
        public string PortName { get; set; }
        public string PortType { get; set; }
        public string CityCode { get; set; }
    }
}
