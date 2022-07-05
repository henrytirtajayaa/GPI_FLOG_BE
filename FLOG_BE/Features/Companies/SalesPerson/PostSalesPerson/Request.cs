using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.SalesPerson.PostSalesPerson
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestSalesPersonBody Body { get; set; }
    }

    public class RequestSalesPersonBody
    {
        public string SalesCode { get; set; }
        public string SalesName { get; set; }
        public Guid PersonId { get; set; }
    }
}
