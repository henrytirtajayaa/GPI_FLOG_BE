using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.JournalEntry.PostJournalEntry
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestEntryHeader Body { get; set; }
    }

    public class RequestEntryHeader
    {
        public DateTime TransactionDate { get; set; }
        public string BranchCode { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public string SourceDocument { get; set; }
        public string Description { get; set; }
        public decimal OriginatingTotal { get; set; }
        public decimal FunctionalTotal { get; set; }

        public List<RequestEntryDetail> RequestEntryDetails { get; set; }

    }

    public class RequestEntryDetail
    {
        public string AccountId { get; set; }
        public string Description { get; set; }
        public decimal OriginatingDebit { get; set; }
        public decimal OriginatingCredit { get; set; }
        public decimal FunctionalDebit { get; set; }
        public decimal FunctionalCredit { get; set; }        
        public int Status { get; set; }
        public int RowIndex { get; set; }
    }
}
