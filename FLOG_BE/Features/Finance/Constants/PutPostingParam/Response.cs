using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Finance.Constants.PutPostingParam
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ResponseItem> PostingParams { get; set; }
    }

    public class ResponseItem
    {
        public int ParamId { get; set; }
        public int PostingKey { get; set; }
        public string AccountId { get; set; }
        public string Description { get; set; }
    }
}
