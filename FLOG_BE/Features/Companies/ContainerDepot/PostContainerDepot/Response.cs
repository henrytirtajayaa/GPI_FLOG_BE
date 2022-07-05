using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.ContainerDepot.PostContainerDepot
{
    public class Response
    {
        public Guid ContainerDepotId { get; set; }
        public string DepotCode { get; set; }
        public string DepotName { get; set; }
       
    }


}
