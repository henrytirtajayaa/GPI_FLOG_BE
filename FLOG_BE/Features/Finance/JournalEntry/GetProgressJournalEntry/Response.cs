using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Finance.JournalEntry.GetProgressJournalEntry
{
    public class Response
    {
        public List<ResponseItem> JournalEntries { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid JournalEntryHeaderId { get; set; }
        public string DocumentNo { get; set; }
        public string BranchCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public string SourceDocument { get; set; }
        public string Description { get; set; }
        public decimal OriginatingTotal { get; set; }
        public decimal FunctionalTotal { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedByName { get; set; }
        public string VoidBy { get; set; }
        public string VoidByName { get; set; }
        public DateTime? VoidDate { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }     

        public int DecimalPlaces { get; set; }
    }
}
