using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.BankReconcile.GetActivitiesBankReconcile
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public string CheckbookCode { get; set; }
        public DateTime? BankCutoffEnd { get; set; }
        public Guid? BankReconcileId { get; set; }
    }
}
