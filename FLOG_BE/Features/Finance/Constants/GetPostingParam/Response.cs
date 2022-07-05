using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.Constants.GetPostingParam
{
    public class Response
    {
        public List<ResponseHeader> PostingParams { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseHeader
    {
        public string Group { get; set; }
        public List<ResponseItem> Params { get; set; }
    }

    public class ResponseItem
    {
        public int ParamId { get; set; }
        public int PostingKey { get; set; }
        public string AccountId { get; set; }
        public string Description { get; set; }
        public string AccountDesc { get; set; }
    }
}
