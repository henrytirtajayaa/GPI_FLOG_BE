using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Finance.Sales.Quotation.GetQuotationProgress
{
    public class Response
    {
        public List<ResponseItem> SalesQuotationHeader { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid SalesQuotationId { get; set; }
        public Int64 RowId { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentNo { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddressCode { get; set; }
        public string SalesCode { get; set; }
        public string SalesPerson { get; set; }
        public Guid ShipperId { get; set; }
        public string ShipperCode { get; set; }
        public string ShipperName { get; set; }
        public string ShipperAddressCode { get; set; }
        public string ConsigneeName { get; set; }
        public Guid ConsigneeId { get; set; }
        public string ConsigneeCode { get; set; }
        public string ConsigneeAddressCode { get; set; }
        public bool IsDifferentNotifyPartner { get; set; }
        public Guid NotifyPartnerId { get; set; }
        public string NotifyPartnerAddressCode { get; set; }
        public string NotifyPartnerCode { get; set; }
        public string NotifyPartnerName { get; set; }
        public Guid ShippingLineId { get; set; }
        public bool IsShippingLineMaster { get; set; }
        public string ShippingLineCode { get; set; }
        public string ShippingLineName { get; set; }
        public string ShippingLineVesselCode { get; set; }
        public string ShippingLineVesselName { get; set; }
        public string ShippingLineOwner { get; set; }
        public string ShippingLineType { get; set; }
        public string ShippingLineShippingNo { get; set; }
        public DateTime ShippingLineDelivery { get; set; }
        public DateTime ShippingLineArrival { get; set; }
        public Guid FeederLineId { get; set; }
        public bool IsFeederLineMaster { get; set; }
        public string FeederLineCode { get; set; }
        public string FeederLineName { get; set; }
        public string FeederLineVesselCode { get; set; }
        public string FeederLineVesselName { get; set; }
        public string FeederLineOwner { get; set; }
        public string FeederLineType { get; set; }
        public string FeederLineShippingNo { get; set; }
        public DateTime FeederLineDelivery { get; set; }
        public DateTime FeederLineArrival { get; set; }
        public string TermOfShipment { get; set; }
        public string FinalDestination { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDischarge { get; set; }
        public string Commodity { get; set; }
        public string CargoGrossWeight { get; set; }
        public string CargoNetWeight { get; set; }
        public string CargoDescription { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }
        public int TotalContainer { get; set; }

        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<SalesQuotationDetail> SalesQuotationDetails { get; set; }

    }
    public class SalesQuotationDetail
    {
        public Guid SalesQuotationDetailId { get; set; }
        public Int64 RowId { get; set; }
        public Guid SalesQuotationId { get; set; }
        public Guid ContainerId { get; set; }
        public string ContainerCode { get; set; }
        public string ContainerName { get; set; }
        public int Qty { get; set; }
        public Guid UomDetailId { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
    }

}
