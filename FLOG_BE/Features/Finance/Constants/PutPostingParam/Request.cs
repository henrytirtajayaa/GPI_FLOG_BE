using Infrastructure.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Finance.Constants.PutPostingParam
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public List<RequestItem> PostingParams { get; set; }
    }

    public class RequestItem
    {
        public int ParamId { get; set; }
        public int PostingKey { get; set; }
        public string AccountId { get; set; }
        public string Description { get; set; }
    }
}
