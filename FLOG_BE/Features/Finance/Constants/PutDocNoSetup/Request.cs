using Infrastructure.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Finance.Constants.PutDocNoSetup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public List<RequestItem> DocNumberSetups { get; set; }
        public List<RequestItem> TrxTypeSetups { get; set; }

    }

    public class RequestItem
    {
        public int DocNumberId { get; set; }
        public int TrxModule { get; set; }
        public string TransactionType { get; set; }
        public int DocFeatureId { get; set; }
        public string DocNo { get; set; }
        public string Description { get; set; }
        public string TrxModuleCaption { get; set; }
        public List<RequestItemApproval> ApprovalSetups { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class RequestItemApproval
    {
        public int TrxModule { get; set; }
        public string TransactionType { get; set; }
        public int DocFeatureId { get; set; }
        public int ModeStatus { get; set; }
        public string ApprovalCode { get; set; }
    }
}
