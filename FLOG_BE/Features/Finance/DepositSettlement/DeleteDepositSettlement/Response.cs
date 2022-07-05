using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Finance.DepositSettlement.DeleteDepositSettlement
{
    public class Response
    {
        public Guid SettlementHeaderId { get; set; }
        public int Status { get; set; }
    }
}
