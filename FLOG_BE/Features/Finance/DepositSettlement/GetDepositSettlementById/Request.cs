using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.DepositSettlement.GetDepositSettlementById
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public Guid SettlementHeaderId { get; set; }
    }
}
