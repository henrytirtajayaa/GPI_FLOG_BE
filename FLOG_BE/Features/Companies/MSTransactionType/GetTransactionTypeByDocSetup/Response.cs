using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.MSTransactionType.GetTransactionTypeByDocSetup
{
    public class Response
    {
        public List<ResponseDocType> DocumentTypes { get; set; }
        public List<ResponseItem> TransactionTypes { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public int DocFeatureId { get; set; }
        public Guid TransactionTypeId { get; set; }
        public string TransactionType { get; set; }
        public string TransactionName { get; set; }
        public int PaymentCondition { get; set; }
        public int RequiredSubject { get; set; }
        public bool InActive { get; set; }
    }

    public class ResponseDocType
    {
        public int DocFeatureId { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
    }
}
