using Infrastructure.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Companies.Country.PostCountry
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyPostCountry Body { get; set; }
    }

    public class RequestBodyPostCountry
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public bool InActive { get; set; }
    }
}
