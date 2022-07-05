using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.ARApply.PutStatusApplyReceivable
{
    public class Response
    {
        public Guid ReceivableApplyId { get; set; }
        public string Message { get; set; }
    }


}
