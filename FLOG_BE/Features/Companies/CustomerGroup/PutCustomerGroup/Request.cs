using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.CustomerGroup.PutCustomerGroup
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutBody Body { get; set; }
    }

    public class RequestPutBody
    {
        public Guid CustomerGroupId { get; set; }
        public string CustomerGroupCode { get; set; }
        public string CustomerGroupName { get; set; }
        public string PaymentTermCode { get; set; }
        public string ReceivableAccountNo { get; set; }
        public string AccruedReceivableAccountNo { get; set; }
        public bool Inactive { get; set; }

    }
}
