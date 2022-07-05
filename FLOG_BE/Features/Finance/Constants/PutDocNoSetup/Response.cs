using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Finance.Constants.PutDocNoSetup
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<ResponseItem> DocNumberSetups { get; set; }

        public List<ResponseItem> TrxTypeSetups { get; set; }
    }

    public class ResponseItem
    {
        public int DocNumberId { get; set; }
        public int TrxModule { get; set; }
        public string TransactionType { get; set; }
        public int DocFeatureId { get; set; }
        public string DocNo { get; set; }
        public string Description { get; set; }
        public List<ResponseItemApproval> ApprovalSetups { get; set; }
        public string TrxModuleCaption { get; set; }
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
}
