using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.Constants.GetDocNoSetup
{
    public class Response
    {
        public List<ResponseHeader> DocNumberSetups { get; set; }
        public List<ResponseHeader> TrxTypeSetups { get; set; }
        public List<ResponseTrxModule> TrxModules { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseHeader
    {
        public int TrxModule { get; set; }
        public string Group { get; set; }        
        public List<ResponseItem> Features { get; set; }
    }

    public class ResponseItem
    {
        public int DocNumberId { get; set; }
        public int TrxModule { get; set; }
        public string TransactionType { get; set; }
        public int DocFeatureId { get; set; }
        public string DocNo { get; set; }
        public string Description { get; set; }
        public string TrxModuleCaption { get; set; }

        public List<ResponseItemApproval> ApprovalSetups { get; set; }
    }

    public class ResponseItemApproval
    {
        public int DocNumberSetupApprovalId { get; set; }
        public int TrxModule { get; set; }
        public string TransactionType { get; set; }
        public int DocFeatureId { get; set; }
        public int ModeStatus { get; set; }
        public string ApprovalCode { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class ResponseTrxModule
    {
        public int TrxModule { get; set; }
        public string Caption { get; set; }
    }
}
