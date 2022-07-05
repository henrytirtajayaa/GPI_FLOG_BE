using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;
using System.ComponentModel.DataAnnotations.Schema;
using SalesOrderPut = FLOG_BE.Features.Finance.Sales.SalesOrder.PutSalesOrder;

namespace FLOG_BE.Features.Sales.NegotiationSheet.PutNegotiationSheet
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestNegotiationSheetBody Body { get; set; }
    }

    public class RequestNegotiationSheetBody
    {
        public Guid NegotiationSheetId { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }     
        public string BranchCode { get; set; }
        public Guid SalesOrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerAddressCode { get; set; }
        public string CustomerBillToAddressCode { get; set; }
        public string CustomerShipToAddressCode { get; set; }
        public string SalesCode { get; set; }
        public string QuotDocumentNo { get; set; }
        public Guid ShipperId { get; set; }
        public string ShipperAddressCode { get; set; }
        public string ShipperBillToAddressCode { get; set; }
        public string ShipperShipToAddressCode { get; set; }
        public string ShipmentStatus { get; set; }
        public string MasterNo { get; set; }
        public string AgreementNo { get; set; }
        public string BookingNo { get; set; }
        public string HouseNo { get; set; }
        public Guid ConsigneeId { get; set; }
        public string ConsigneeAddressCode { get; set; }
        public string ConsigneeBillToAddressCode { get; set; }
        public string ConsigneeShipToAddressCode { get; set; }
        public bool IsDifferentNotifyPartner { get; set; }
        public Guid NotifyPartnerId { get; set; }
        public string NotifyAddressCode { get; set; }
        public string NotifyBillToAddressCode { get; set; }
        public string NotifyShipToAddressCode { get; set; }
        public Guid ShippingLineId { get; set; }
        public bool IsShippingLineMaster { get; set; }
        public string ShippingLineCode { get; set; }
        public string ShippingLineName { get; set; }
        public string ShippingLineShippingNo { get; set; }
        public string ShippingLineVesselCode { get; set; }
        public string ShippingLineVesselName { get; set; }
        public DateTime ShippingLineETD { get; set; }
        public DateTime ShippingLineETA { get; set; }
        public Guid FeederLineId { get; set; }
        public string TermOfShipment { get; set; }
        public string FinalDestination { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDischarge { get; set; }
        public string Commodity { get; set; }
        public string CargoGrossWeight { get; set; }
        public string CargoNetWeight { get; set; }
        public string CargoDescription { get; set; }

        public decimal TotalFuncSelling { get; set; }
        public decimal TotalFuncBuying { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }
        public List<NsSelling> NsSellings { get; set; }
        public List<NsContainer> NsContainers { get; set; }
        public List<NsTrucking> NsTruckings { get; set; }
        
    }

    public class NsContainer
    {
        public Guid SalesOrderContainerId { get; set; }
        public Int64 RowId { get; set; }
        public int RowIndex { get; set; }
        public Guid ContainerId { get; set; }
        public string ContainerCode { get; set; }
        public string ContainerName { get; set; }
        public int Qty { get; set; }
        public Guid UomDetailId { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }

    }
    public class NsTrucking
    {
        public Guid SalesOrderTruckingId { get; set; }
        public Int64 RowId { get; set; }
        public int RowIndex { get; set; }
        public Guid VehicleTypeId { get; set; }
        public string TruckloadTerm { get; set; }
        public Guid VendorId { get; set; }
        public int Qty { get; set; }
        public Guid UomDetailId { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }

    }
    public class NsSelling
    {
        public Guid SalesOrderSellingId { get; set; }
        public Int64 RowId { get; set; }
        public int RowIndex { get; set; }
        public Guid ChargeId { get; set; }
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
        public decimal PercentDiscount { get; set; }
        public int PaymentCondition { get; set; }
        public Guid CustomerId { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public List<NsBuying> NsBuyings { get; set; }

    }
    public class NsBuying
    {
        public Guid SalesOrderBuyingId { get; set; }
        public Int64 RowId { get; set; }
        public int RowIndex { get; set; }
        public Guid NsSellingId { get; set; }
        public Guid ChargeId { get; set; }
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
        public decimal PercentDiscount { get; set; }
        public int PaymentCondition { get; set; }
        public Guid VendorId { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }

    }


}
