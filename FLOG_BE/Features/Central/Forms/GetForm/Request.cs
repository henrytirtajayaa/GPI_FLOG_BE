using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.SecurityRoles.Forms.GetForm
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public RequestFilterForm Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilterForm
    {
        public List<string> FormId { get; set; }
        public List<string> FormName { get; set; }
        public List<string> Module { get; set; }

    }
}
