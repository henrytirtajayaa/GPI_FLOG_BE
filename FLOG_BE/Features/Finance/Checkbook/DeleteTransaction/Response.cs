using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Finance.Checkbook.DeleteTransaction
{
    public class Response
    {
        public Guid CheckbookTransactionId { get; set; }
        public String Message { get; set; }
    }
}
