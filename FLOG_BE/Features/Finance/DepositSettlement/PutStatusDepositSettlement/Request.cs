using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.DepositSettlement.PutStatusDepositSettlement
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutStatus Body { get; set; }
    }

    public class RequestPutStatus
    {
        public Guid SettlementHeaderId { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }
        public DateTime ActionDate { get; set; }
    }
}
