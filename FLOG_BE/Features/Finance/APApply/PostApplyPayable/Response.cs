using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Finance.APApply.PostApplyPayable
{
    public class Response
    {
        public Guid PayableApplyId { get; set; }
        public string DocumentNo { get; set; }
    
    }
}
