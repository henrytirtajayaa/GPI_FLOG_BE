using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Finance.ArReceipt.DeleteCustomerReceipt
{
    public class Response
    {
        public Guid ReceiptHeaderId { get; set; }
        public int Status { get; set; }
    }
}
