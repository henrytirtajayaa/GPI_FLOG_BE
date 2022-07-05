using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOG_BE.Features.Finance.GLStatement.PutGLStatementDetail
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestFormBody Body { get; set; }
    }

    public class RequestFormBody
    {
        public int DetailId { get; set; }
        public int SubCategoryId { get; set; }
        public string AccountName { get; set; }
        public int PosIndex { get; set; }
        public bool IsCashflow { get; set; }
        public bool ShowZeroValue { get; set; }
        public bool IsCashflowDynamic { get; set; }
        public List<string> AccountIds { get; set; }
    }

}
