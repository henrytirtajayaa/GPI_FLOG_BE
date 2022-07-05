using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Rental.ContainerRequestConfirm.PostContainerRequestConfirm
{
    public class Response
    {
        public Guid ContainerRequestConfirmHeaderId { get; set; }
        public string DeliveryOrderNo { get; set; }
    }
}
