using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Rental.ContainerRequestConfirm.GetProgress
{
    public class Response
    {
        public List<ResponseItem> ContainerRequestConfirm { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid ContainerRequestConfirmHeaderId { get; set; }
        public Guid ContainerRentalRequestHeaderId { get; set; }
        public string TransactionType { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DocumentNo { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string AddressCode { get; set; }
        public int Status { get; set; }
        public string SalesCode { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }
        public string BillToAddressCode { get; set; }
        public string ShipToAddressCode { get; set; }
        public string DeliveryOrderNo { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<ContainerRequestConfirmDetail> ContainerRequestConfirmDetails { get; set; }
    }

    public class ContainerRequestConfirmDetail
    {
        public Guid ContainerRequestConfirmDetailId { get; set; }
        public Guid ContainerRequestConfirmHeaderId { get; set; }
        public Guid ContainerRentalRequestDetailId { get; set; }
        public string ContainerCode { get; set; }
        public string ContainerName { get; set; }
        public string UomCode { get; set; }
        public string Remarks { get; set; }
        public string Grade { get; set; }
        public int Quantity { get; set; }
        public int QuantityRemaining { get; set; }
        public int QuantityToConfirm { get; set; }
        public int QuantityBalance { get; set; }
    }
}
