using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.MSDepartment.PostMsDepartment
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPostMsDepartment Body { get; set; }
    }

    public class RequestPostMsDepartment
    {
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public bool Inactive { get; set; }
    }
}
