using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class NegotiationSheetHeader
    {
        public NegotiationSheetHeader()
        {
            #region Generated Constructor
            #endregion
        }
        #region Generated Properties
        public Guid NegotiationSheetId { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentNo { get; set; }
        public string BranchCode { get; set; }
        public Guid SalesOrderId { get; set; }
        public decimal TotalFuncSelling { get; set; }
        public decimal TotalFuncBuying { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public string StatusComment { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Guid CustomerId { get; set; }
        public string CustomerAddressCode { get; set; }
        public string CustomerBillToAddressCode { get; set; }
        public string CustomerShipToAddressCode { get; set; }
        public string QuotDocumentNo { get; set; }
        public string SalesCode { get; set; }
        public string ShipmentStatus { get; set; }  
        public string MasterNo { get; set; }
        public string AgreementNo { get; set; }
        public string BookingNo { get; set; }
        public string HouseNo { get; set; }
        public Guid ShipperId { get; set; }
        public string ShipperAddressCode { get; set; }
        public string ShipperBillToAddressCode { get; set; }
        public string ShipperShipToAddressCode { get; set; }
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
        public string ShippingLineShippingNo { get; set; }
        public DateTime? ShippingLineETD { get; set; }
        public DateTime? ShippingLineETA { get; set; }
        public string TermOfShipment { get; set; }
        public string FinalDestination { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDischarge { get; set; }
        public string Commodity { get; set; }
        public string CargoGrossWeight { get; set; }
        public string CargoNetWeight { get; set; }
        public string CargoDescription { get; set; }
        public bool IsShippingLineMaster { get; set; }
        public string ShippingLineCode { get; set; }
        public string ShippingLineVesselCode { get; set; }
        public string ShippingLineVesselName { get; set; }
        public string ShippingLineName { get; set; }

        #endregion

        [NotMapped]
        public virtual List<NegotiationSheetContainer> NsContainers { get; set; }
        [NotMapped]
        public virtual List<NegotiationSheetTrucking> NsTruckings { get; set; }
        [NotMapped]
        public virtual List<NegotiationSheetSelling> NsSellings { get; set; }
        [NotMapped]
        public virtual List<NegotiationSheetBuying> NsBuyings { get; set; }
        [NotMapped]
        public decimal TotalFuncProfit { get; set; }
        [NotMapped]
        public string SoDocumentNo { get; set; }
        [NotMapped]
        public string SalesName { get; set; }
        [NotMapped]
        public string CustomerCode { get; set; }
        [NotMapped]
        public string CustomerName { get; set; }
        [NotMapped]
        public string ShipperCode { get; set; }
        [NotMapped]
        public string ShipperName { get; set; }
        [NotMapped]
        public string ConsigneeCode { get; set; }
        [NotMapped]
        public string ConsigneeName { get; set; }
        [NotMapped]
        public string NotifyPartnerCode { get; set; }
        [NotMapped]
        public string NotifyPartnerName { get; set; }
        [NotMapped]
        public string ShippingLineOwner { get; set; }
        [NotMapped]
        public string ShippingLineType { get; set; }
        [NotMapped]
        public virtual bool IsInvoicePending { get; set; }
        [NotMapped]
        public virtual List<FLOG.Core.DocumentNo.TrxPersonApprover> CurrentApprovers { get; set; }

        [NotMapped]
        public virtual List<Helper.ActionButton> ActionButtons { get; set; }

        [NotMapped]
        public virtual string _cellClasses { get; set; }
    }
}
