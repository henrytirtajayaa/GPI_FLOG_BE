using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.Company.DeleteCompany
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyDelete Body { get; set; }
    }

    public class RequestBodyDelete
    {
        public string CompanyId { get; set; }
    }
}
