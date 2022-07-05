using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.MSTransactionType.DeleteTransactionType
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestDeleteTransactionType Body { get; set; }
    }

    public class RequestDeleteTransactionType
    { 
        public Guid TransactionTypeId { get; set; }
    }
}
