using FLOG_BE.Model.Companies.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.CustomerGroup.DeleteCustomerGroup
{
    public class Response
    {
        public Guid CustomerGroupId { get; set; }
        public int Deleted { get; set; }
    }
}
