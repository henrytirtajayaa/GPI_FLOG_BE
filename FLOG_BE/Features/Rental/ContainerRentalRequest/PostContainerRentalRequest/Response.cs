using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Rental.ContainerRentalRequest.PostContainerRentalRequest
{
    public class Response
    {
        public Guid ContainerRentalRequestHeaderId { get; set; }
        public string DocumentNo { get; set; }
    }
}
