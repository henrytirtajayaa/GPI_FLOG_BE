using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Finance.APApply.PutApplyPayable
{

    public class Response
    {
        public Guid PayableApplyId { get; set; }
        public string DocumentType { get; set; }
        public string Message { get; set; }
    }
}
