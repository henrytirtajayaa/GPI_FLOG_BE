using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Companies.Port.PutPort
{
    public class Response
    {
        public string PortCode { get; set; }
        public string PortName { get; set; }
        public string PortType { get; set; }
        public string CityCode { get; set; }
        public bool InActive { get; set; }
    }
}
