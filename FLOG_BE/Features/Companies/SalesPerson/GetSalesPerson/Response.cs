using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.SalesPerson.GetSalesPerson
{
    public class Response
    {
        public List<ResponseItem> SalesPersons { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid SalesPersonId { get; set; }
        public string SalesCode { get; set; }
        public string SalesName { get; set; }
        public Guid PersonId { get; set; }
        public string PersonFullName { get; set; }
    }
}
