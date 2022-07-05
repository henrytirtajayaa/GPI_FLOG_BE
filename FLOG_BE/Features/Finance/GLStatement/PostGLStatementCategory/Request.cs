using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOG_BE.Features.Finance.GLStatement.PostGLStatementCategory
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestFormBody Body { get; set; }
    }

    public class RequestFormBody
    {
        public int StatementType { get; set; }
        public string CategoryKey { get; set; }
        public string CategoryCaption { get; set; }     
        public List<RequestFormItem> SubCategories { get; set; }

    }

    public class RequestFormItem
    {
        public string SubCategoryKey { get; set; }
        public string SubCategoryCaption { get; set; }
        public bool IsParamTotal { get; set; }
        public bool Inflow { get; set; }
        public int PosIndex { get; set; }
    }
}
