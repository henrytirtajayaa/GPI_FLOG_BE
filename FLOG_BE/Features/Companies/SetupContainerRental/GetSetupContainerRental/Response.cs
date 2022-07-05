using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Companies.SetupContainerRental.GetSetupContainerRental
{
    public class Response
    {
        public List<ResponseItem> SetupContainerRentals { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
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
    }
}
