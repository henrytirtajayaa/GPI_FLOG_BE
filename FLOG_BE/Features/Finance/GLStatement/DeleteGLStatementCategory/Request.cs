using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Finance.GLStatement.DeleteGLStatementCategory
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestDeleteBody Body { get; set; }
    }

    public class RequestDeleteBody
    {
        public int CategoryId { get; set; }

    }

}
