using System;
using System.Collections.Generic;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.Sales.SalesOrder.GetSalesOrderProgress
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public string SecuritySetting { get; set; }
        public RequestFilter Filter { get; set; }
        public Int32 Offset { get; set; }
        public Int32 Limit { get; set; }
        public List<string> Sort { get; set; }
    }

    public class RequestFilter
    {
      
        public List<Guid> SalesOrderId { get; set; }
        public Int64 RowId { get; set; }
        public List<DateTime?> TransactionDateStart { get; set; }
        public List<DateTime?> TransactionDateEnd { get; set; }
        public List<string> DocumentNo { get; set; }
        public List<string> BranchCode { get; set; }

        public List<string> SalesPerson { get; set; }
        public List<Guid> CustomerId { get; set; }
        public List<string> CustomerName { get; set; }
        public List<string> CustomerAddressCode { get; set; }
        public List<string> CustomerBillToAddressCode { get; set; }
        public List<string> CustomerShipToAddressCode { get; set; }
        public List<string> SalesCode { get; set; }
        public List<string> QuotDocumentNo { get; set; }
        public List<string> ShipmentStatus { get; set; }
        public List<string> MasterNo { get; set; }
        public List<string> AgreementNo { get; set; }
        public List<string> BookingNo { get; set; }
        public List<string> HouseNo { get; set; }

        public List<Guid> ShipperId { get; set; }
        public List<string> ShipperName { get; set; }
        public List<string> ShippingLineOwner { get; set; }
        public List<string> ShipperBillToAddressCode { get; set; }
        public List<string> ShipperShipToAddressCode { get; set; }
        public List<Guid> ConsigneeId { get; set; }
        public List<string> ConsigneeAddressCode { get; set; }
        public List<string> ConsigneeBillToAddressCode { get; set; }
        public List<string> ConsigneeShipToAddressCode { get; set; }

        public bool? IsDifferentNotifyPartner { get; set; }
        public List<Guid> NotifyPartnerId { get; set; }
        public List<string> NotifyPartnerAddressCode { get; set; }
        public List<string> NotifyPartnerCode { get; set; }
        public List<string> NotifyPartnerName { get; set; }
        public List<string> NotifyPartnerBilltoAddressCode { get; set; }
        public List<string> NotifyPartnerShipToAddressCode { get; set; }
        public List<Guid> ShippingLineId { get; set; }
        public List<string> ShippingLineShippingNo { get; set; }
        public List<DateTime> ShippingLineDelivery { get; set; }
        public List<DateTime> ShippingLineArrival { get; set; }
        public List<Guid> FeederLineId { get; set; }
        public List<string> FeederLineShippingNo { get; set; }
        public List<DateTime> FeederLineDelivery { get; set; }
        public List<DateTime> FeederLineArrival { get; set; }
        public List<string> TermOfShipment { get; set; }
        public List<string> FinalDestination { get; set; }
        public List<string> PortOfLoading { get; set; }
        public List<string> PortOfDischarge { get; set; }
        public List<string> Commodity { get; set; }
        public List<string> CargoGrossWeight { get; set; }
        public List<string> CargoNetWeight { get; set; }
        public List<string> CargoDescription { get; set; }
        public List<string> Remark { get; set; }
        public int? Status { get; set; }
        public List<string> StatusComment { get; set; }

        public List<string> CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<string> ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<string> CreatedByName { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> ModifiedByName { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
      
    }
}
