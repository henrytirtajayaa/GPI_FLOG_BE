using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.MSTransactionType.PutTransactionType
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestUpdateTransactionType Body { get; set; }
    }

    public class RequestUpdateTransactionType
    {
        public string TransactionTypeId { get; set; }
        public string TransactionType { get; set; }
        public string TransactionName { get; set; }
        public int PaymentCondition { get; set; }
        public int RequiredSubject { get; set; }
        public bool InActive { get; set; }
    }
}
