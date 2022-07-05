using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class SalesQuotationHeader
    {
        public SalesQuotationHeader()
        {
            #region Generated Constructor
            #endregion
        }
        #region Generated Properties

        public Guid SalesQuotationId { get; set; }
        public Int64 RowId { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentNo { get; set; }
        public Guid CustomerId { get; set; }
        [NotMapped]
        public string CustomerCode { get; set; }
        [NotMapped]
        public string CustomerName { get; set; }
        [NotMapped]
        public string ShipperCode { get; set; }
        [NotMapped]
        public string ShipperName { get; set; }
        public string CustomerAddressCode { get; set; }
        public string SalesCode { get; set; }
        [NotMapped]
        public string SalesPerson { get; set; }
        public Guid ShipperId { get; set; }
        public string ShipperAddressCode { get; set; }
        public Guid ConsigneeId { get; set; }
        [NotMapped]
        public string ConsigneeCode { get; set; }
        [NotMapped]
        public string ConsigneeName { get; set; }
        public string ConsigneeAddressCode { get; set; }
        public bool IsDifferentNotifyPartner { get; set; }
        public Guid NotifyPartnerId { get; set; }
        public string NotifyPartnerAddressCode { get; set; }
        [NotMapped]
        public string NotifyPartnerName { get; set; }
        [NotMapped]
        public string NotifyPartnerCode { get; set; }
        public Guid ShippingLineId { get; set; }
        public bool IsShippingLineMaster { get; set; }
        public string ShippingLineCode { get; set; }
        public string ShippingLineName { get; set; }
        public string ShippingLineVesselCode { get; set; }
        public string ShippingLineVesselName { get; set; }
        [NotMapped]
        public string ShippingLineOwner { get; set; }
        [NotMapped]
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

        [NotMapped]
        public string FeederLineOwner { get; set; }
        [NotMapped]
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
        public string Remark { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }
        [NotMapped]
        public string CreatedByName { get; set; }
        [NotMapped]
        public string ModifiedByName { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        [NotMapped]
        public int TotalContainer { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion
        [NotMapped]
        public virtual List<SalesQuotationDetail> SalesQuotationDetails { get; set; }
    }
}
