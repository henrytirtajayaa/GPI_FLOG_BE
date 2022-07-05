using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Finance.ArReceipt.PostCustomerReceipt
{
    public class Response
    {
        public Guid ReceiptHeaderId { get; set; }
        public string TransactionType { get; set; }
        public string DocumentNo { get; set; }
    }
}
