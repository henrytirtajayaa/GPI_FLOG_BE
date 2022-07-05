using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Rental.ContainerRequestConfirm.PostContainerRequestConfirm
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestContainerRequestConfirm Body { get; set; }
    }

    public class RequestContainerRequestConfirm
    {
        public Guid ContainerRentalRequestHeaderId { get; set; }
        public DateTime DocumentDate { get; set; }
        public int Status { get; set; }
        public string DeliveryOrderNo { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public List<RequestContainerRequestConfirmDetail> ContainerRequestConfirmDetails { get; set; }
    }

    public class RequestContainerRequestConfirmDetail
    {
        public Guid ContainerRequestConfirmHeaderId { get; set; }
        public Guid ContainerRentalRequestDetailId { get; set; }
        public string Remarks { get; set; }
        public string Grade { get; set; }
        public int QuantityToConfirm { get; set; }
        public int QuantityBalance { get; set; }
    }
}
