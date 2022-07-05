using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.TaxSchedule.GetTaxSchedule

{
    public class Response
    {
        public List<ResponseItem> TaxSchedules { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public string TaxScheduleId { get; set; }
        public string TaxScheduleCode { get; set; }
        public string Description { get; set; }
        public string IsSales { get; set; }
        public decimal PercentOfSalesPurchase { get; set; }
        public decimal TaxablePercent { get; set; }
        public string RoundingType { get; set; }
        public decimal RoundingLimitAmount { get; set; }
        public bool TaxInclude { get; set; }
        public bool WithHoldingTax { get; set; }

        public string TaxAccountNo { get; set; }
        public string Inactive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
