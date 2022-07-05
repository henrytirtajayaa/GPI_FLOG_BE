using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.Constants.GetCalcMethod
{
    public class Response
    {
        public List<ResponseItem> CalcMethods { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public int CalcMethod { get; set; }
        public string Caption { get; set; }
    }
}
