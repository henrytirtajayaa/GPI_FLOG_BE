using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.GLStatement.GetGLStatementDetail
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public int StatementType { get; set; }

        public List<string> CategoryKey { get; set; }
        public List<string> CategoryCaption { get; set; }
        public List<string> SubCategoryKey { get; set; }
        public List<string> SubCategoryCaption { get; set; }
        public List<string> AccountName { get; set; }
    }
}
