using Infrastructure.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Companies.Country.PutCountry
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyUpdateCountry Body { get; set; }
    }

    public class RequestBodyUpdateCountry
    { 
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public bool InActive { get; set; }
    }
}
