using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOG_BE.Features.Finance.Sales.Quotation.SubmitQuotation
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public SubmitQuotationBody Body { get; set; }
    }

    public class SubmitQuotationBody
    {
        public Guid SalesQuotationId { get; set; }
        public Int64 RowId { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentNo { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerAddressCode { get; set; }
        public string SalesCode { get; set; }
        public Guid ShipperId { get; set; }
        public string ShipperAddressCode { get; set; }
        public Guid ConsigneeId { get; set; }
        public string ConsigneeAddressCode { get; set; }
        public bool IsDifferentNotifyPartner { get; set; }
        public Guid NotifyPartnerId { get; set; }
        public string NotifyPartnerAddressCode { get; set; }
        public Guid ShippingLineId { get; set; }
        public string ShippingLineShippingNo { get; set; }
        public DateTime? ShippingLineDelivery { get; set; }
        public DateTime? ShippingLineArrival { get; set; }
        public Guid FeederLineId { get; set; }
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
        public string Remark { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<SalesQuotationDetail> SalesQuotationDetails { get; set; }

      
    }

    public class SalesQuotationDetail
    {
        public Guid SalesQuotationDetailId { get; set; }
        public Int64 RowId { get; set; }
        public Guid SalesQuotationId { get; set; }
        public Guid ContainerId { get; set; }
        public int Qty { get; set; }
        public Guid UomDetailId { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
    }


}
