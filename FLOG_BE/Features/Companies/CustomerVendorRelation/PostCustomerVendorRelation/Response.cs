using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.CustomerVendorRelation.PostCustomerVendorRelation
{
    public class Response
    {
        public Guid RelationId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid VendorId { get; set; }
    }


}
