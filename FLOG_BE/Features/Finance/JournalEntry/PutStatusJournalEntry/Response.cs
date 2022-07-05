using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.JournalEntry.PutStatusJournalEntry
{
    public class Response
    {
        public Guid JournalEntryHeaderId { get; set; }
        public string Message { get; set; }
    }


}
