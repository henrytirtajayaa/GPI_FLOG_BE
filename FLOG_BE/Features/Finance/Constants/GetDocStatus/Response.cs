using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.Constants.GetDocStatus
{
    public class Response
    {
        public List<ResponseItem> DocStatus { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public int DocStatus { get; set; }
        public string Caption { get; set; }
    }
}
