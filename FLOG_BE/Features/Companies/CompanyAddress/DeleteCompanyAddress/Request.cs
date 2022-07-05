using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.CompanyAddress.DeleteCompanyAddress
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyDeleteCompanyAddress Body { get; set; }
    }

    public class RequestBodyDeleteCompanyAddress
    { 
        public string CompanyAddressId { get; set; }
    }
}
