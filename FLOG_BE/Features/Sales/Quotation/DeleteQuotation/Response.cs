using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Sales.Quotation.DeleteQuotation
{
    public class Response
    {
        public Guid SalesQuotationId { get; set; }
        public String Message { get; set; }
    }
}
