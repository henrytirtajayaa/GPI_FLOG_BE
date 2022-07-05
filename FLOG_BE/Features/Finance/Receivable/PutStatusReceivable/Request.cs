using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Features.Finance.Receivable.PutReceivableTransaction;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Receivable.PutStatusReceivable
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestReceivable Body { get; set; }

    }

}
