using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.GLStatement.GetGLStatementCategory
{
    public class Response
    {
        public List<ResponseItem> Categories { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public int CategoryId { get; set; }
        public int StatementType { get; set; }
        public string CategoryKey { get; set; }
        public string CategoryCaption { get; set; }
        
        public List<RequestFormSub> SubCategories { get; set; }
    }

    public class RequestFormSub
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryKey { get; set; }
        public string SubCategoryCaption { get; set; }
        public bool IsParamTotal { get; set; }
        public bool Inflow { get; set; }
        public int PosIndex { get; set; }
    }

}
