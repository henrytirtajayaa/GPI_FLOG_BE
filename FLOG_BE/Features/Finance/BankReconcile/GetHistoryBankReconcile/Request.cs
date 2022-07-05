using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.BankReconcile.GetHistoryBankReconcile
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public List<DateTime?> TransactionDateStart { get; set; }
        public List<DateTime?> TransactionDateEnd { get; set; }
        public List<string> CheckbookCode { get; set; }
        public List<string> DocumentNo { get; set; }
        public List<string> CurrencyCode { get; set; }
        public List<DateTime?> BankCutoffEndStart { get; set; }
        public List<DateTime?> BankCutoffEndEnd { get; set; }
        public List<string> Description { get; set; }
        public List<decimal?> BankEndingOrgBalanceMin { get; set; }
        public List<decimal?> BankEndingOrgBalanceMax { get; set; }
        public List<decimal?> CheckbookEndingOrgBalanceMin { get; set; }
        public List<decimal?> CheckbookEndingOrgBalanceMax { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<string> CreatedByName { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<string> ModifiedByName { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
        public int? Status { get; set; }
        public List<string> StatusComment { get; set; }
    }
}
