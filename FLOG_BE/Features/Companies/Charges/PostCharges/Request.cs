using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Charges.PostCharges
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestChargesBody Body { get; set; }
    }

    public class RequestChargesBody
    {
        public string ChargesCode { get; set; }
        public string ChargeGroupCode { get; set; }
        public string ChargesName { get; set; }
        public string TransactionType { get; set; }
        public bool IsPurchasing { get; set; }
        public bool IsSales { get; set; }
        public bool IsInventory { get; set; }
        public bool IsFinancial { get; set; }
        public bool IsAsset { get; set; }
        public bool IsDeposit { get; set; }
        public string RevenueAccountNo { get; set; }
        public string TempRevenueAccountNo { get; set; }
        public string CostAccountNo { get; set; }
        public string TaxScheduleCode { get; set; }
        public string ShippingLineType { get; set; }
        public bool HasCosting { get; set; }
        public bool InActive { get; set; }

        public List<RequestChargesDetail> ChargesDetails { get; set; }

    }

    public class RequestChargesDetail
    {
        public Guid ShippingLineId { get; set; }
        public string RevenueAccountNo { get; set; }
        public string TempRevenueAccountNo { get; set; }
        public string CostAccountNo { get; set; }

    }
}
