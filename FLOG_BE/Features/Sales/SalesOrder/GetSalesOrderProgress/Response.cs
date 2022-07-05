﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Finance.Sales.SalesOrder.GetSalesOrderProgress
{
    public class Response
    {
        public List<ResponseItem> SalesOrderHeader { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid SalesOrderId { get; set; }
        public Int64 RowId { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentNo { get; set; }
        public string BranchCode { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ShipperCode { get; set; }
        public string ShipperName { get; set; }
        public string CustomerAddressCode { get; set; }
        public string CustomerBillToAddressCode { get; set; }
        public string CustomerShipToAddressCode { get; set; }
        public string SalesCode { get; set; }
        public string QuotDocumentNo { get; set; }
        public string ShipmentStatus { get; set; }
        public string SalesPerson { get; set; }
        public string MasterNo { get; set; }
        public string AgreementNo { get; set; }
        public string BookingNo { get; set; }
        public string HouseNo { get; set; }

        public Guid ShipperId { get; set; }
        public string ShipperAddressCode { get; set; }
        public string ShipperBillToAddressCode { get; set; }
        public string ShipperShipToAddressCode { get; set; }
        public Guid ConsigneeId { get; set; }
        public string ConsigneeCode { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddressCode { get; set; }
        public string ConsigneeBillToAddressCode { get; set; }
        public string ConsigneeShipToAddressCode { get; set; }
        public bool IsDifferentNotifyPartner { get; set; }
        public Guid NotifyPartnerId { get; set; }
        public string NotifyPartnerCode { get; set; }
        public string NotifyPartnerName { get; set; }
        public string NotifyPartnerAddressCode { get; set; }
        public string NotifyPartnerBilltoAddressCode { get; set; }
        public string NotifyPartnerShipToAddressCode { get; set; }
        public Guid ShippingLineId { get; set; }
        public bool IsShippingLineMaster { get; set; }
        public string ShippingLineCode { get; set; }
        public string ShippingLineName { get; set; }
        public string ShippingLineVesselCode { get; set; }
        public string ShippingLineVesselName { get; set; }
        public string ShippingLineOwner { get; set; }
        public string ShippingLineType { get; set; }
        public string ShippingLineShippingNo { get; set; }
        public DateTime? ShippingLineDelivery { get; set; }
        public DateTime? ShippingLineArrival { get; set; }
        public Guid FeederLineId { get; set; }
        public bool IsFeederLineMaster { get; set; }
        public string FeederLineCode { get; set; }
        public string FeederLineName { get; set; }
        public string FeederLineVesselCode { get; set; }
        public string FeederLineVesselName { get; set; }
        public string FeederLineOwner { get; set; }
        public string FeederLineType { get; set; }
        public string FeederLineShippingNo { get; set; }
        public DateTime? FeederLineDelivery { get; set; }
        public DateTime? FeederLineArrival { get; set; }
        public string TermOfShipment { get; set; }
        public string FinalDestination { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDischarge { get; set; }
        public string Commodity { get; set; }
        public string CargoGrossWeight { get; set; }
        public string CargoNetWeight { get; set; }
        public string CargoDescription { get; set; }
        public decimal FunctionalSellingAmount { get; set; }
        public decimal FunctionalBuyingAmount { get; set; }
        public decimal EstimatedFunctionalAmount { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public int TotalContainer { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public List<SalesOrderContainer> SalesOrderContainers { get; set; }
        public List<SalesOrderTrucking> SalesOrderTruckings { get; set; }
        public List<SalesOrderSelling> SalesOrderSellings { get; set; }
        public List<SalesOrderBuying> SalesOrderBuyings { get; set; }

    }
    public class SalesOrderContainer
    {
        public Guid SalesOrderContainerId { get; set; }
        public Int64 RowId { get; set; }
        public Guid SalesOrderId { get; set; }
        public Guid ContainerId { get; set; }
        public string ContainerCode { get; set; }
        public string ContainerName { get; set; }
        public int Qty { get; set; }
        public Guid UomDetailId { get; set; }
        public int Status { get; set; }

    }
    public class SalesOrderTrucking
    {
        public Guid SalesOrderTruckingId { get; set; }
        public Int64 RowId { get; set; }
        public Guid SalesOrderId { get; set; }
        public Guid VehicleTypeId { get; set; }
        public string TruckloadTerm { get; set; }
        public Guid VendorId { get; set; }
        public int Qty { get; set; }
        public Guid UomDetailId { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }

        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string VehicleTypeCode { get; set; }
        public string VehicleTypeName { get; set; }
    }
    public class SalesOrderSelling
    {
        public Guid SalesOrderSellingId { get; set; }
        public Int64 RowId { get; set; }
        public Guid SalesOrderId { get; set; }
        public Guid ChargeId { get; set; }
        public string ChargeCode { get; set; }
        public string ChargeName { get; set; }
        public Guid ScheduleId { get; set; }
        public string ScheduleCode { get; set; }
        //public decimal PercentDiscount { get; set; }
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
        public decimal UnitAmount { get; set; }
        public decimal Quantity { get; set; }
        public List<SalesOrderBuying> SoBuyings { get; set; }
    

    }
  
    public class SalesOrderBuying
    {
        public Guid SalesOrderBuyingId { get; set; }
        public Int64 RowId { get; set; }
        public Guid SalesOrderId { get; set; }
        public Guid SalesOrderSellingId { get; set; }
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
        public decimal TaxablePercentTax { get; set; }
        public bool IsTaxAfterDiscount { get; set; }
        public decimal PercentDiscount { get; set; }
        public int PaymentCondition { get; set; }
        public Guid VendorId { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public decimal UnitAmount { get; set; }
        public decimal Quantity { get; set; }
        public Guid ScheduleId { get; set; }

    }

}
