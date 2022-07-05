using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.GLStatement.GetGLStatementDetail
{
    public class Response
    {
        public List<ResponseItem> Details { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public int DetailId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string AccountName { get; set; }
        public string SubCategoryKey { get; set; }
        public string SubCategoryCaption { get; set; }                
        public int PosIndex { get; set; }
        public string CategoryKey { get; set; }
        public string CategoryCaption { get; set; }
        public bool IsCashflow { get; set; }
        public bool ShowZeroValue { get; set; }
        public bool IsCashflowDynamic { get; set; }
        public List<ResponseItemSub> DetailAccounts { get; set; }
    }

    public class ResponseItemSub
    {
        public string AccountId { get; set; }
        public string Description { get; set; }

    }
}
