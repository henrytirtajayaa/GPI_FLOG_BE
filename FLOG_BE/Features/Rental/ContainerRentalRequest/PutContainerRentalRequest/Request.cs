using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Rental.ContainerRentalRequest.PutContainerRentalRequest
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestContainerRentalRequest Body { get; set; }
    }

    public class RequestContainerRentalRequest
    {
        public Guid ContainerRentalRequestHeaderId { get; set; }
        public string TransactionType { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DocumentNo { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string AddressCode { get; set; }
        public int Status { get; set; }
        public string SalesCode { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public string BillToAddressCode { get; set; }
        public string ShipToAddressCode { get; set; }
        public List<RequestContainerRentalRequestDetail> ContainerRentalRequestDetails { get; set; }
    }

    public class RequestContainerRentalRequestDetail
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
