using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Checkbook.PutTrxDelete
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestCheckbookTrxDeleteBody Body { get; set; }
    }

    public class RequestCheckbookTrxDeleteBody
    {
        public Guid CheckbookTransactionId { get; set; }

    }

}
