using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.CustomerAddress.DeleteCustomerAddress
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyDeleteCustomerAddress Body { get; set; }
    }

    public class RequestBodyDeleteCustomerAddress
    { 
        public string CustomerAddressId { get; set; }
    }
}
