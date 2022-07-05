using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Finance.Sales.SalesOrder.PutStatusSalesOrder
{
    public class Response
    {
        public Guid SalesOrderId { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
