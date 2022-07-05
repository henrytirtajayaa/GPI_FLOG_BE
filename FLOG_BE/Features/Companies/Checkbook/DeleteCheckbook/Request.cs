using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Checkbook.DeleteCheckbook
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyDeleteCheckbook Body { get; set; }
    }

    public class RequestBodyDeleteCheckbook
    { 
        public string CheckbookId { get; set; }
    }
}
