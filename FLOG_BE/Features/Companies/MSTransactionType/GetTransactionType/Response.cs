using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.MSTransactionType.GetTransactionType
{
    public class Response
    {
        public List<ResponseItem> TransactionTypes { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid TransactionTypeId { get; set; }
        public string TransactionType { get; set; }
        public string TransactionName { get; set; }
        public int PaymentCondition { get; set; }
        public int RequiredSubject { get; set; }
        public bool InActive { get; set; }
    }
}
