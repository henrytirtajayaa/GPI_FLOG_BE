using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Features.Finance.JournalEntry.PostJournalEntry;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.JournalEntry.PutJournalEntry
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestEntryHeader Body { get; set; }

    }

    public class RequestEntryHeader
    {
        public Guid JournalEntryHeaderId { get; set; }
        public string BranchCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public string SourceDocument { get; set; }
        public string Description { get; set; }
        public decimal OriginatingTotal { get; set; }
        public decimal FunctionalTotal { get; set; }

        public List<RequestEntryDetail> RequestEntryDetails { get; set; }

        public int ActionDocStatus { get; set; }

        public string Comments { get; set; }
        public DateTime ActionDate { get; set; }
    }
}
