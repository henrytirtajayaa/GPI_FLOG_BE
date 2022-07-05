using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.ApPayment.GetUserApprovalPayment
{
    public class Response
    {
        public ResponseData UserApproval { get; set; }
        public List<ResponseItem> DetailEntries { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid PaymentApprovalId { get; set; }
        public Guid PaymentHeaderId { get; set; }

        public int Index { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? PersonCategoryId { get; set; }
        public int Status { get; set; }
        public Int64 RowId { get; set; }

    }

    public class ResponseData
    {
        public Guid PaymentApprovalId { get; set; }
        public int MaxIndex { get; set; }
        public int CurrentIndex { get; set; }
        public bool StatusApproval { get; set; }
        public bool StatusPosting { get; set; }
        public Guid? PersonId { get; set; }
    }

}
