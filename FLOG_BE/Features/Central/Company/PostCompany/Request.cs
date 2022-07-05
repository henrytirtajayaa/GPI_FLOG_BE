using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.Company.PostCompany
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public string CompanyName { get; set; }
        public string DatabaseId { get; set; }
        public string DatabaseAddress { get; set; }
        public string DatabasePassword { get; set; }
        public string CoaSymbol { get; set; }
        public int CoaTotalLength { get; set; }
        public bool InActive { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
    }
}
