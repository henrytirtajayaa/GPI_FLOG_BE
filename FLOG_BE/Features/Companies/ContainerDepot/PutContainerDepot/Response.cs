using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.ContainerDepot.PutContainerDepot
{
    public class Response
    {
        public string DepotCode { get; set; }
        public string DepotName { get; set; }
    }
}
