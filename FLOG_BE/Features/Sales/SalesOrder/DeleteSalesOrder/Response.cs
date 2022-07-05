using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Sales.SalesOrder.DeleteSalesOrder
{
    public class Response
    {
        public Guid SalesOrderId { get; set; }
        public String Message { get; set; }
    }
}
