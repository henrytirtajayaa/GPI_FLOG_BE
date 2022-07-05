using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.SalesPerson.PutSalesPerson
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutSalesPersonBody Body { get; set; }
    }

    public class RequestPutSalesPersonBody
    {
        public Guid SalesPersonId { get; set; }
        public string SalesCode { get; set; }
        public string SalesName { get; set; }
        public Guid PersonId { get; set; }

    }
}
