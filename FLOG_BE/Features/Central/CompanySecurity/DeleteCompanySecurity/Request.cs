using Infrastructure.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Central.CompanySecurity.DeleteCompanySecurity
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }

        public RequestBodyDeleteCompanySecurity Body { get; set; }
    }

    public class RequestBodyDeleteCompanySecurity
    { 
        public string CompanySecurityId { get; set; }
    }
}
