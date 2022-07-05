using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.SalesPerson.GetSalesPerson
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {

        public List<string> SalesCode { get; set; }
        public List<string> SalesName { get; set; }
        public List<Guid> PersonId { get; set; }
    }
}
