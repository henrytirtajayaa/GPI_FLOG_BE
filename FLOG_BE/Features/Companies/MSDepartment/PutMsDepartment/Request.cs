using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.MSDepartment.PutMsDepartment
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestUpdateDepartment Body { get; set; }
    }

    public class RequestUpdateDepartment
    {
        public Guid DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public bool Inactive { get; set; }
    }
}
