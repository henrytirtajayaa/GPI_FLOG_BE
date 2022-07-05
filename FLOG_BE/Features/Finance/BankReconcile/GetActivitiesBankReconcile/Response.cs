using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.BankReconcile.GetActivitiesBankReconcile
{
    public class Response
    {
        public List<ResponseItem> CheckbookActivities { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid TransactionId { get; set; }
        public string Modul { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public string CheckbookCode { get; set; }
        public string PaidSubject { get; set; }
        public string SubjectCode { get; set; }
        public string BankAccountCode { get; set; }
        public string Description { get; set; }
        public decimal OriginatingTotalAmount { get; set; }
        public decimal OriginatingDebit { get; set; }
        public decimal OriginatingCredit { get; set; }
        public bool IsChecked { get; set; }
    }

}
