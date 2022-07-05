using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.MSTransactionType.GetTransactionType
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public List<string> TransactionTypeId { get; set; }
        public List<string> TransactionType { get; set; }
        public List<string> TransactionName { get; set; }
        public List<int?> PaymentCondition { get; set; }
        public List<int?> RequiredSubject { get; set; }
        public bool? InActive { get; set; }
    }
}
