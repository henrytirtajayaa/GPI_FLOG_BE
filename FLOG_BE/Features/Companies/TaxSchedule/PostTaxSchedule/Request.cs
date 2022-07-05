using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.TaxSchedule.PostTaxSchedule
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestTaxScheduleBody Body { get; set; }
    }

    public class RequestTaxScheduleBody
    {
      
        public string TaxScheduleCode { get; set; }
        public string Description { get; set; }
        public bool IsSales { get; set; }
        public decimal PercentOfSalesPurchase { get; set; }
        public decimal TaxablePercent { get; set; }
        public byte RoundingType { get; set; }
        public decimal RoundingLimitAmount { get; set; }
        public bool TaxInclude { get; set; }
        public bool WithHoldingTax { get; set; }

        public string TaxAccountNo { get; set; }
        public bool Inactive { get; set; }

    }
}
