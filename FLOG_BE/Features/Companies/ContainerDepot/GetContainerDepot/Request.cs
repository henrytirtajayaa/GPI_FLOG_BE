using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ContainerDepot.GetContainerDepot
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
        public List<string> DepotCode { get; set; }
        public List<string> DepotName { get; set; }
        public List<string> VendorName { get; set; }
        public List<string> CityName { get; set; }
        public bool? InActive { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
    }
}
