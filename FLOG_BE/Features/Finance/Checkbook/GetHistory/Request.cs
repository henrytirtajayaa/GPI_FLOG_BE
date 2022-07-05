using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Checkbook.GetHistory
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string Checkbooks { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {

        public List<string> DocumentType { get; set; }
        public List<string> DocumentNo { get; set; }
        public List<string> BranchCode { get; set; }
        public List<DateTime?> TransactionDateStart { get; set; }
        public List<DateTime?> TransactionDateEnd { get; set; }
        public List<string> TransactionType { get; set; }
        public List<string> CurrencyCode { get; set; }
        public List<decimal?> ExchangeRateMin { get; set; }
        public List<decimal?> ExchangeRateMax { get; set; }
        public List<string> CheckbookCode { get; set; }
        public bool? IsVoid { get; set; }
        public List<string> VoidDocumentNo { get; set; }
        public List<string> PaidSubject { get; set; }
        public List<string> SubjectCode { get; set; }
        public List<string> Description { get; set; }
        public List<decimal?> OriginatingTotalAmountMax { get; set; }
        public List<decimal?> OriginatingTotalAmountMin { get; set; }
        public List<decimal?> FunctionalTotalAmountMax { get; set; }
        public List<decimal?> FunctionalTotalAmountMin { get; set; }
        public List<string> VoidBy { get; set; }
        public List<DateTime?> VoidDateStart { get; set; }
        public List<DateTime?> VoidDateEnd { get; set; }
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
