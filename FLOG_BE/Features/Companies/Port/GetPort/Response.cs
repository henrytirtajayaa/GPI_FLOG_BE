using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Companies.Port.GetPort
{
    public class Response
    {
        public List<ResponseItem> Ports { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    { 
        public string PortId { get; set; }
        public string PortCode { get; set; }
        public string PortName { get; set; }
        public string PortType { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string Province { get; set; }
        public string CountryName { get; set; }
        public bool InActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
