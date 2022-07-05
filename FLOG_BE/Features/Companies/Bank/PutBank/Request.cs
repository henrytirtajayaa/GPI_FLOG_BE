using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Bank.PutBank
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBankUpdate Body { get; set; }
    }

    public class RequestBankUpdate
    {
        public string BankId { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string Address { get; set; }
        public string CityCode { get; set; }
        public bool Inactive { get; set; }
    }
}
