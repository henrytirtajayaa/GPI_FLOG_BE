using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Sales.Quotation.DeleteQuotation
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodySalesQuotation Body { get; set; }
    }

    public class RequestBodySalesQuotation
    {
        public Guid SalesQuotationId { get; set; }
    }
}
