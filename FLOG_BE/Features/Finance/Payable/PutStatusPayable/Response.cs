using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Finance.Payable.PutStatusPayable
{
    public class Response
    {
        public Guid PayableTransactionId { get; set; }
        public string Message { get; set; }
    }
}
