using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Sales.NegotiationSheet.GetNegotiationSheetApprovalComment
{
    public class Response
    {
        public List<ResponseItem> ApprovalComments { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid TrxDocumentApprovalCommentId { get; set; }
        public Guid TransactionId { get; set; }
        public Guid PersonId { get; set; }
        public DateTime CommentDate { get; set; }
        public string Comments { get; set; }
        public int Status { get; set; }
        public string UserFullName { get; set; }

    }
}
