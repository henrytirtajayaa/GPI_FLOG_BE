using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.Vendor.PutVendor
{
    public class Response
    {
        public Guid VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
    }


}
