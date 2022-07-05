using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Bank.DeleteBank
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBankDelete Body { get; set; }
    }

    public class RequestBankDelete
    {
        public String BankId { get; set; }
    }
}
