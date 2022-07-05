using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.JournalEntry.DeleteJournalEntry
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestDeleteJournalEntry Body { get; set; }
    }

    public class RequestDeleteJournalEntry
    {
        public Guid JournalEntryHeaderId { get; set; }
    }
}
