using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.CompanyBranch.PutCompanyBranch
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutUpdate Body { get; set; }
    }

    public class RequestPutUpdate
    {
        public Guid CompanyBranchId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public bool Default { get; set; }
        public bool Inactive { get; set; }
    }
}
