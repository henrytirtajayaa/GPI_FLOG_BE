﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.SetupContainerRental.PutSetupContainerRental
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyUpdateSetupContainerRental Body { get; set; }
    }

    public class RequestBodyUpdateSetupContainerRental
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
    }
}
