using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class ContainerRentalRequestHeader
    {
        public ContainerRentalRequestHeader()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid ContainerRentalRequestHeaderId { get; set; }
        public string TransactionType { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DocumentNo { get; set; }
        public Guid CustomerId { get; set; }
        [NotMapped]
        public string CustomerCode { get; set; }
        [NotMapped]
        public string CustomerName { get; set; }
        public string AddressCode { get; set; }
        public int Status { get; set; }
        public string SalesCode { get; set; }
        public Guid VendorId { get; set; }
        [NotMapped]
        public string VendorCode { get; set; }
        [NotMapped]
        public string VendorName { get; set; }
        public string BillToAddressCode { get; set; }
        public string ShipToAddressCode { get; set; }
        public string CreatedBy { get; set; }
        [NotMapped]
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        [NotMapped]
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CanceledBy { get; set; }
        [NotMapped]
        public string CanceledByName { get; set; }
        public DateTime? CanceledDate { get; set; }
        public List<ContainerRentalRequestDetail> ContainerRentalRequestDetails { get; set; }
        #endregion
    }
}
