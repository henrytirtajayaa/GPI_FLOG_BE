using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.JournalEntry.PostJournalEntry
{
    public class Response
    {
        public Guid JournalEntryHeaderId { get; set; }
        public string DocumentNo { get; set; }
        public string Message { get; set; }
    }
}
