using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Sales.SalesOrder.PutStatusSalesOrder
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestSalesOrder Body { get; set; }

    }

    public class RequestSalesOrder
    {
    
        public Guid SalesOrderId { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }

    }
   
}
