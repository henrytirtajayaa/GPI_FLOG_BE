using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.NumberFormatHeader.DeleteNumberFormatHeader
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyDeleteNumberFormatHeader Body { get; set; }
    }

    public class RequestBodyDeleteNumberFormatHeader
    { 
        public string FormatHeaderId { get; set; }
    }
}
