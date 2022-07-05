using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.SetupContainerRental.GetSetupContainerRental
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
        public List<string> TransactionType { get; set; }
        public List<string> RequestDocNo { get; set; }
        public List<string> DeliveryDocNo { get; set; }
        public List<string> ClosingDocNo { get; set; }
        public List<string> UomScheduleCode { get; set; }
        public int? CustomerFreeUsageDays { get; set; }
        public int? ShippingLineFreeUsageDays { get; set; }
        public int? CntOwnerFreeUsageDays { get; set; }
    }
}
