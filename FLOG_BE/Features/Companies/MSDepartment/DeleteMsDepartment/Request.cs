using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.MSDepartment.DeleteMsDepartment
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestDeleteMsDepartment Body { get; set; }
    }

    public class RequestDeleteMsDepartment
    { 
        public Guid DepartmentId { get; set; }
    }
}
