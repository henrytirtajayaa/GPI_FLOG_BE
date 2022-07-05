using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Charges.GetCharges
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

        public List<string> ChargesCode { get; set; }
        public List<string> ChargeGroupCode { get; set; }
        public List<string> ChargesName { get; set; }
        public List<string> TransactionType { get; set; }
        public bool? IsPurchasing { get; set; }
        public bool? IsSales { get; set; }
        public bool? IsInventory { get; set; }
        public bool? IsFinancial { get; set; }
        public bool? IsAsset { get; set; }
        public bool? IsDeposit { get; set; }
        public List<string> RevenueAccountNo { get; set; }
        public List<string> TempRevenueAccountNo { get; set; }
        public List<string> CostAccountNo { get; set; }
        public List<string> TaxScheduleCode { get; set; }
        public List<string> ShippingLineType { get; set; }
        public bool? HasCosting{ get; set; }
        public bool? InActive { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<DateTime?> ModifiedDate { get; set; }
    }
}
