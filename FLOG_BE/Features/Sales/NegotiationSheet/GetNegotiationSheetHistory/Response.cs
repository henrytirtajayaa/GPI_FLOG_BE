using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Central.Entities;
using Entities = FLOG_BE.Model.Companies.Entities;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Sales.NegotiationSheet.GetNegotiationSheetHistory
{
    public class Response
    {
        public List<Entities.NegotiationSheetHeader> NegotiationSheetHeaders { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid NegotiationSheetId { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string BranchCode { get; set; }
        public string DocumentNo { get; set; }
        public Guid SalesOrderId { get; set; }
        public string SoDocumentNo { get; set; }
        public string CustomerName { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public decimal TotalFuncBuying { get; set; }
        public decimal TotalFuncSelling { get; set; }
        public decimal TotalFuncProfit { get; set; }
        public Entities.SalesOrderHeader SalesOrder { get; set; }

        public List<NsContainer> NsContainers { get; set; }
        public List<NsTrucking> NsTruckings { get; set; }
        public List<NsSelling> NsSellings { get; set; }

    }
    
    public class NsContainer
    {
        public Guid NsContainerId { get; set; }
        public Int64 RowId { get; set; }
        public Guid SalesOrderId { get; set; }
        public Guid ContainerId { get; set; }
        public string ContainerCode { get; set; }
        public string ContainerName { get; set; }
        public int Qty { get; set; }
        public Guid UomDetailId { get; set; }
        public int Status { get; set; }

    }
    public class NsTrucking
    {
        public Guid NsTruckingId { get; set; }
        public Int64 RowId { get; set; }
        public Guid VehicleTypeId { get; set; }
        public string TruckloadTerm { get; set; }
        public Guid VendorId { get; set; }
        public int Qty { get; set; }
        public Guid UomDetailId { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public string VehicleTypeCode { get; set; }
        public string VehicleTypeName { get; set; }

    }
    public class NsSelling
    {
        public Guid NsSellingId { get; set; }
        public Int64 RowId { get; set; }
        public Guid ChargeId { get; set; }
        public string ChargeCode { get; set; }
        public string ChargeName { get; set; }
        public string ScheduleCode { get; set; }
        public decimal TaxablePercentTax { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public decimal OriginatingAmount { get; set; }
        public decimal OriginatingTax { get; set; }
        public decimal OriginatingDiscount { get; set; }
        public decimal OriginatingExtendedAmount { get; set; }
        public decimal FunctionalTax { get; set; }
        public decimal FunctionalDiscount { get; set; }
        public decimal FunctionalExtendedAmount { get; set; }
        public Guid TaxScheduleId { get; set; }
        public bool IsTaxAfterDiscount { get; set; }
        public int DecimalPlaces { get; set; }
        public decimal PercentDiscount { get; set; }
        public int PaymentCondition { get; set; }
        public Guid CustomerId { get; set; }
        public string ChargeTo { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public List<NsBuying> NsBuyings { get; set; }
    }

    public class NsBuying
    {
        public Guid NsBuyingId { get; set; }
        public Int64 RowId { get; set; }
        public Guid NsSellingId { get; set; }
        public Guid ChargeId { get; set; }
        public string ChargeCode { get; set; }
        public string ChargeName { get; set; }
        public string ChargeTo { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsMultiply { get; set; }
        public int DecimalPlaces { get; set; }
        public decimal OriginatingAmount { get; set; }
        public decimal OriginatingTax { get; set; }
        public decimal OriginatingDiscount { get; set; }
        public decimal OriginatingExtendedAmount { get; set; }
        public decimal FunctionalTax { get; set; }
        public decimal FunctionalDiscount { get; set; }
        public decimal FunctionalExtendedAmount { get; set; }
        public Guid TaxScheduleId { get; set; }
        public string ScheduleCode { get; set; }
        public decimal TaxablePercentTax { get; set; }
        public bool IsTaxAfterDiscount { get; set; }
        public decimal PercentDiscount { get; set; }
        public int PaymentCondition { get; set; }
        public Guid VendorId { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public Guid ScheduleId { get; set; }

    }
}
