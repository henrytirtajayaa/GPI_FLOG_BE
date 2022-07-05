using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Sales.NegotiationSheet.DeleteNegotiationSheet
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyNegotiationSheet Body { get; set; }
    }

    public class RequestBodyNegotiationSheet
    {
        public Guid NegotiationSheetId { get; set; }
    }
}
