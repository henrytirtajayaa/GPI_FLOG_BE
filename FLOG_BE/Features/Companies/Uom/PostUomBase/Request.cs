using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Uom.PostUomBase
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestUomBaseBody Body { get; set; }
    }

    public class RequestUomBaseBody
    {
        public string UomCode { get; set; }
        public string UomName { get; set; }
   
    }
}
