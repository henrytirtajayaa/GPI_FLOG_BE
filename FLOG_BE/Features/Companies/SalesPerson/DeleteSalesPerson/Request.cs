using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.SalesPerson.DeleteSalesPerson
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestSalesPersonDelete Body { get; set; }
    }

    public class RequestSalesPersonDelete
    {
        public Guid SalesPersonId { get; set; }
    }
}
