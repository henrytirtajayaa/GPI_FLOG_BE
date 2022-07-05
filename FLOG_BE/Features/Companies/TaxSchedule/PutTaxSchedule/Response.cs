using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.TaxSchedule.PutTaxSchedule
{
    public class Response
    {
        public Guid TaxScheduleId { get; set; }
        public string TaxScheduleCode { get; set; }
        public string Description { get; set; }
        public bool IsSales { get; set; }
        public decimal PercentOfSalesPurchase { get; set; }
        public decimal TaxablePercent { get; set; }
        public byte RoundingType { get; set; }
        public decimal RoundingLimitAmount { get; set; }
        public bool TaxInclude { get; set; }
        public bool WithHoldingTax { get; set; }
        public bool Inactive { get; set; }


    }


}
