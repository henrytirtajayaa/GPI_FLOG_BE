using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Sales.Quotation.PutStatusQuotation
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestSalesCancel Body { get; set; }

    }

    public class RequestSalesCancel
    {
    
        public Guid SalesQuotationId { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }
       
       
    }
   
}
