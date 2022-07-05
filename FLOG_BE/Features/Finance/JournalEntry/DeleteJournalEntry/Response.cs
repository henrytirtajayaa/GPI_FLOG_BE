using FLOG_BE.Model.Companies.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.JournalEntry.DeleteJournalEntry
{
    public class Response
    {
        public Guid JournalEntryHeaderId { get; set; }
        public String Message { get; set; }
    }
}
