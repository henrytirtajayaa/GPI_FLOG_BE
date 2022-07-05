using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PutJournalEntry = FLOG_BE.Features.Finance.JournalEntry.PutJournalEntry;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.JournalEntry.PutStatusJournalEntry
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public PutJournalEntry.RequestEntryHeader Body { get; set; }
    }
}
