using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Finance.DepositSettlement.PutStatusDepositSettlement
{
    public class Response
    {
        public Guid SettlementHeaderId { get; set; }
        public string Message { get; set; }
    }
}
