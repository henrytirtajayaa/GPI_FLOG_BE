using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class ContainerRequestConfirmHeader
    {
        public ContainerRequestConfirmHeader()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid ContainerRequestConfirmHeaderId { get; set; }
        public Guid ContainerRentalRequestHeaderId { get; set; }
        [NotMapped] public string TransactionType { get; set; }
        public DateTime DocumentDate { get; set; }
        [NotMapped] public string DocumentNo { get; set; }
        [NotMapped] public Guid CustomerId { get; set; }
        [NotMapped] public string CustomerCode { get; set; }
        [NotMapped] public string CustomerName { get; set; }
        [NotMapped] public string AddressCode { get; set; }
        public int Status { get; set; }
        [NotMapped] public string SalesCode { get; set; }
        [NotMapped] public Guid VendorId { get; set; }
        [NotMapped] public string VendorCode { get; set; }
        [NotMapped] public string VendorName { get; set; }
        [NotMapped] public string BillToAddressCode { get; set; }
        [NotMapped] public string ShipToAddressCode { get; set; }
        public string DeliveryOrderNo { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string CreatedBy { get; set; }
        [NotMapped] public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        [NotMapped] public string ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [NotMapped] public List<ContainerRequestConfirmDetail> ContainerRequestConfirmDetails { get; set; }
        #endregion
    }
}
