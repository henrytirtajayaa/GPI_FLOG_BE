using Infrastructure.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Companies.Country.DeleteCountry
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyDeleteCountry Body { get; set; }
    }

    public class RequestBodyDeleteCountry
    {
        public string CountryId { get; set; }
    }
}
