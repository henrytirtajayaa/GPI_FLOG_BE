using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.JournalEntry.GetDetailJournalEntry
{
    public class Response
    {
        public List<ResponseItem> DetailEntries { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid JournalEntryHeaderId { get; set; }
        public Guid JournalEntryDetailId { get; set; }
        public string AccountId { get; set; }
        public string AccountDescription { get; set; }
        public string Description { get; set; }
        public decimal OriginatingDebit { get; set; }
        public decimal OriginatingCredit { get; set; }
        public decimal FunctionalDebit { get; set; }
        public decimal FunctionalCredit { get; set; }
        public int Status { get; set; } 

    }
}
