using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Rental.ContainerRequestConfirm.PutContainerRequestConfirm
{
    public class Response
    {
        public Guid ContainerRequestConfirmHeaderId { get; set; }
        public DateTime DocumentDate { get; set; }
        public int Status { get; set; }
        public string DeliveryOrderNo { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
