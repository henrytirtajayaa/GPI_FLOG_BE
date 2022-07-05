using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.ExchangeRateHeader.DeleteExchangeRate
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyDeleteExchangeRate Body { get; set; }
    }

    public class RequestBodyDeleteExchangeRate
    {
        public Guid ExchangeRateHeaderId { get; set; }
    }
}
