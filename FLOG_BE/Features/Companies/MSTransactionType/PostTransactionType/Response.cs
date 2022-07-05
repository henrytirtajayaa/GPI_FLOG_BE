using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Companies.MSTransactionType.PostTransactionType
{
    public class Response
    {
        public Guid TransactionTypeId { get; set; }
        public string TransactionType { get; set; }
        public string TransactionName { get; set; }
        public int PaymentCondition { get; set; }
        public int RequiredSubject { get; set; }
        public int TrxModule { get; set; }
        public bool InActive { get; set; }
    }
}
