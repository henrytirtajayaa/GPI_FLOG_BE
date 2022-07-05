using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Sales.SalesOrder.DeleteSalesOrder
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodySalesOrder Body { get; set; }
    }

    public class RequestBodySalesOrder
    {
        public Guid SalesOrderId { get; set; }
    }
}
