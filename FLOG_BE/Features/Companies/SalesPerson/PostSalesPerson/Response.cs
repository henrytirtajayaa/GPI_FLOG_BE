using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.SalesPerson.PostSalesPerson
{
    public class Response
    {
        public Guid SalesPersonId { get; set; }
        public string SalesCode { get; set; }
        public string SalesName { get; set; }
        public Guid PersonId { get; set; }

    }


}
