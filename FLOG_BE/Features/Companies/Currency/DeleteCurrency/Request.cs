using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Currency.DeleteCurrency
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestCurrencyDelete Body { get; set; }
    }

    public class RequestCurrencyDelete
    {
        public Guid CurrencyId { get; set; }
    }
}
