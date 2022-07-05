using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Central.CompanySecurity.PutCompanySecurity
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestUpdateCS Body { get; set; }
    }

    public class RequestUpdateCS
    {
        public string CompanySecurityId { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string RoleId { get; set; }
    }
}
