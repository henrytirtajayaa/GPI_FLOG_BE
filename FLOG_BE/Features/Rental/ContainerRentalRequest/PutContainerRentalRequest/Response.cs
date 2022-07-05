using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Rental.ContainerRentalRequest.PutContainerRentalRequest
{
    public class Response
    {
        public Guid ContainerRentalRequestHeaderId { get; set; }
        public string TransactionType { get; set; }
        public DateTime DocumentDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string AddressCode { get; set; }
        public int Status { get; set; }
        public string SalesCode { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string BillToAddressCode { get; set; }
        public string ShipToAddressCode { get; set; }
    }
}
