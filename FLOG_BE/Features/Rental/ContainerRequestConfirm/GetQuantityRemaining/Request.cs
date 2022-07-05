using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Rental.ContainerRequestConfirm.GetQuantityRemaining
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
        public Guid ContainerRentalRequestHeaderId { get; set; } // NEW
        public List<string> TransactionType { get; set; }
        public List<DateTime?> DocumentDateStart { get; set; }
        public List<DateTime?> DocumentDateEnd { get; set; }
        public List<string> DocumentNo { get; set; }
        public Guid CustomerId { get; set; }
        public List<string> CustomerCode { get; set; }
        public List<string> CustomerName { get; set; }
        public List<string> AddressCode { get; set; }
        public int? Status { get; set; }
        public List<string> SalesCode { get; set; }
        public Guid VendorId { get; set; }
        public List<string> VendorCode { get; set; }
        public List<string> VendorName { get; set; }
        public List<string> BillToAddressCode { get; set; }
        public List<string> ShipToAddressCode { get; set; }
        public List<string> CreatedBy { get; set; }
        public List<string> CreatedByName { get; set; }
        public List<DateTime?> CreatedDateStart { get; set; }
        public List<DateTime?> CreatedDateEnd { get; set; }
        public List<string> ModifiedBy { get; set; }
        public List<string> ModifiedByName { get; set; }
        public List<DateTime?> ModifiedDateStart { get; set; }
        public List<DateTime?> ModifiedDateEnd { get; set; }
        public List<string> CanceledBy { get; set; }
        public List<string> CanceledByName { get; set; }
        public List<DateTime?> CanceledDateStart { get; set; }
        public List<DateTime?> CanceledDateEnd { get; set; }
    }
}
