using FLOG_BE.Model.Companies.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.Customer.DeleteCustomer
{
    public class Response
    {
        public Guid CustomerId { get; set; }
        public int Deleted { get; set; }
    }
}
