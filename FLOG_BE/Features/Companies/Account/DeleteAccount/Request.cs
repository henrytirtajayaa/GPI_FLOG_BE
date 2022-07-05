using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Account.DeleteAccount
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestAccountDelete Body { get; set; }
    }

    public class RequestAccountDelete
    {
        public String AccountId { get; set; }
    }
}
