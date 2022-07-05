using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.Charges.GetCharges
{
    public class Response
    {
        public List<ResponseItem> Charges { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public string ChargesId { get; set; }
        public string ChargesCode { get; set; }
        public string ChargeGroupCode { get; set; }
        public string ChargeGroupName { get; set; }
        public string TransactionType { get; set; }
        public string ChargesName { get; set; }
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
        public string InActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public List<ResponseChargesDetail> ChargesDetails { get; set; }
    }

    public class ResponseChargesDetail
    {
        public Guid ChargesDetailId { get; set; }
        public Guid ShippingLineId { get; set; }
        public string RevenueAccountNo { get; set; }
        public string TempRevenueAccountNo { get; set; }
        public string CostAccountNo { get; set; }
        public string ShippingLineCode { get; set; }
        public string ShippingLineName { get; set; }
    }
}
