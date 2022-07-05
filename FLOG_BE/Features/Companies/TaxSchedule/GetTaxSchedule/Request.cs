using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.TaxSchedule.GetTaxSchedule
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
        public Guid? TaxScheduleId { get; set; }
        public List<string> TaxScheduleCode { get; set; }
        public List<string> Description { get; set; }
        public List<bool?> IsSales { get; set; }
        public List<decimal?> PercentOfSalesPurchaseMin { get; set; }
        public List<decimal?> PercentOfSalesPurchaseMax { get; set; }
        public List<decimal?> TaxablePercentMin { get; set; }
        public List<decimal?> TaxablePercentMax { get; set; }
        public List<byte?> RoundingType { get; set; }
        public List<decimal?> RoundingLimitAmountMin { get; set; }
        public List<decimal?> RoundingLimitAmountMax { get; set; }
        public List<bool?> TaxInclude { get; set; }
        public List<bool?> WithHoldingTax { get; set; }
        public List<string> TaxAccountNo { get; set; }
        public List<bool?> Inactive { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
    }
}
