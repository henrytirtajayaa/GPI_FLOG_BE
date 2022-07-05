using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Rental.ContainerRentalRequest.PutDeleteContainerRentalRequest
{
    public class Response
    {
        public Guid ContainerRentalRequestHeaderId { get; set; }
        public int Status { get; set; }
    }
}
