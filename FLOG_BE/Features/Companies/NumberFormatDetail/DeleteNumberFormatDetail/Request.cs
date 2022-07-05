using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.NumberFormatDetail.DeleteNumberFormatDetail
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyDeleteNumberFormatDetail Body { get; set; }
    }

    public class RequestBodyDeleteNumberFormatDetail
    { 
        public string FormatDetailId { get; set; }
    }
}
