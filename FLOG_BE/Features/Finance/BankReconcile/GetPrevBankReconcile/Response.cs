using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.BankReconcile.GetPrevBankReconcile
{
    public class Response
    {
        public ResponseItem BankReconcile { get; set; }
        public bool AllowNew { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid BankReconcileId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentNo { get; set; }
        public string CheckbookCode { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime BankCutoffStart { get; set; }
        public DateTime BankCutoffEnd { get; set; }
        public string Description { get; set; }
        public decimal BankEndingOrgBalance { get; set; }
        public decimal CheckbookEndingOrgBalance { get; set; }
        public decimal BalanceDifference { get; set; }
    }
}
