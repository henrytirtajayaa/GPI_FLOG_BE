using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.Company.PutCompany
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyUpdate Body { get; set; }
    }

    public class RequestBodyUpdate
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string DatabaseAddress { get; set; }
        public string DatabasePassword { get; set; }

        public bool InActive { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
    }
}
