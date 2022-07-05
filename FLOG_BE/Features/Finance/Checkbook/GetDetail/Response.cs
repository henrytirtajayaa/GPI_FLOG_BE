using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.Checkbook.GetDetail
{
    public class Response
    {
        public List<ResponseItem> DetailEntries { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid TransactionDetailId { get; set; }
        public Guid CheckbookTransactionId { get; set; }
        public Guid ChargesId { get; set; }
        public string ChargesCode { get; set; }
        public string ChargesName { get; set; }
        public string ChargesDescription { get; set; }
        public decimal OriginatingAmount { get; set; }
        public decimal FunctionalAmount { get; set; }
        public int Status { get; set; }

    }
}
