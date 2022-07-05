using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.ContainerDepot.GetContainerDepot
{
    public class Response
    {
        public List<ResponseItem> ContainerDepots { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid ContainerDepotId { get; set; }
        public string DepotCode { get; set; }
        public string DepotName { get; set; }
        public Guid OwnerVendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string Province { get; set; }
        public string CountryName { get; set; }
        public bool InActive { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
  
}
