using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class SetupContainerRental
    {
        public SetupContainerRental()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid SetupContainerRentalId { get; set; }
        public string TransactionType { get; set; }
        public string RequestDocNo { get; set; }
        public string DeliveryDocNo { get; set; }
        public string ClosingDocNo { get; set; }
        public string UomScheduleCode { get; set; }
        public int CustomerFreeUsageDays { get; set; }
        public int ShippingLineFreeUsageDays { get; set; }
        public int CntOwnerFreeUsageDays { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion

        #region Generated Relationship
        #endregion
    }
}
