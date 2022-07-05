using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.JournalEntry.GetProgressJournalEntry
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
        public List<string> DocumentNo { get; set; }
        public List<DateTime?> TransactionDateStart { get; set; }
        public List<DateTime?> TransactionDateEnd { get; set; }
        public List<string> SourceDocument { get; set; }
        public List<string> BranchCode { get; set; }
        public List<string> CurrencyCode { get; set; }
        public List<decimal?> ExchangeRateMin { get; set; }
        public List<decimal?> ExchangeRateMax { get; set; }
        public List<string> Description { get; set; }
        public List<decimal?> OriginatingTotalMin { get; set; }
        public List<decimal?> OriginatingTotalMax { get; set; }
        public List<decimal?> FunctionalTotalMin { get; set; }
        public List<decimal?> FunctionalTotalMax { get; set; }
        public int? Status { get; set; }
        public List<string> StatusComment { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<string> CreatedName { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<string> ModifiedByName { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }     

    }
}
