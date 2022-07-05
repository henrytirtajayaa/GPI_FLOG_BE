using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Finance.Receivable.PutStatusDeposit
{
    public class Response
    {
        public Guid ReceiveTransactionId { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
