using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.DepositSettlement.DeleteDepositSettlement
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestDeleteDepositSettlement Body { get; set; }
    }

    public class RequestDeleteDepositSettlement
    {
        public Guid SettlementHeaderId { get; set; }
    }
}
