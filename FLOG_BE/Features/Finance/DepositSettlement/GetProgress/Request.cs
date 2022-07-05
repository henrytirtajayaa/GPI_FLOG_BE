using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.DepositSettlement.GetProgress
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public List<DateTime?> TransactionDateStart { get; set; }
        public List<DateTime?> TransactionDateEnd { get; set; }
        public List<string> DocumentType { get; set; }
        public List<string> DocumentNo { get; set; }
        public List<string> DepositNo { get; set; }
        public List<string> CurrencyCode { get; set; }
        public List<decimal?> ExchangeRateMin { get; set; }
        public List<decimal?> ExchangeRateMax { get; set; }
        public List<string> CheckbookCode { get; set; }
        public Guid CustomerId { get; set; }
        public List<string> CustomerName { get; set; }
        public List<string> Description { get; set; }
        public List<decimal?> OriginatingTotalPaidMin { get; set; }
        public List<decimal?> OriginatingTotalPaidMax { get; set; }
        public List<decimal?> FunctionalTotalPaidMin { get; set; }
        public List<decimal?> FunctionalTotalPaidMax { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<string> CreatedByName { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<string> ModifiedByName { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
        public List<string> VoidBy { get; set; }
        public List<string> VoidByName { get; set; }
        public List<DateTime?> VoidDateStart { get; set; }
        public List<DateTime?> VoidDateEnd { get; set; }
        public int? Status { get; set; }
        public List<string> StatusComment { get; set; }
    }
}
