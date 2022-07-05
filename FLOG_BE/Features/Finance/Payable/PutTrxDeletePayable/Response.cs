using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Central.Entities;

namespace FLOG_BE.Features.Finance.Payable.PutTrxDeletePayable
{
    public class Response
    {
        public Guid PayableTransactionId { get; set; }
        public int Status { get; set; }
    }
}
