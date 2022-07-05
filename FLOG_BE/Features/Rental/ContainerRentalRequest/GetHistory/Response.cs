using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Rental.ContainerRentalRequest.GetHistory
{
    public class Response
    {
        public List<ResponseItem> ContainerRentalRequest { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
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
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string BillToAddressCode { get; set; }
        public string ShipToAddressCode { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CanceledBy { get; set; }
        public string CanceledByName { get; set; }
        public DateTime? CanceledDate { get; set; }
        public List<ContainerRentalRequestDetail> ContainerRentalRequestDetails { get; set; }
    }

    public class ContainerRentalRequestDetail
    {
        public Guid ContainerRentalRequestDetailId { get; set; }
        public Guid ContainerRentalRequestHeaderId { get; set; }
        public string ContainerCode { get; set; }
        public string ContainerName { get; set; }
        public string UomCode { get; set; }
        public string Remarks { get; set; }
        public int Quantity { get; set; }
    }
}
