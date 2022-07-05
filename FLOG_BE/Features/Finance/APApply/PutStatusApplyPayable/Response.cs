using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.APApply.PutStatusApplyPayable
{
    public class Response
    {
        public Guid PayableApplyId { get; set; }
        public string Message { get; set; }
    }


}
