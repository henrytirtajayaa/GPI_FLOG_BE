using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.CompanySecurity.PostCompanySecurity
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyCS Body { get; set; }
    }

    public class RequestBodyCS
    {
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string RoleId { get; set; }
    
    }
}
