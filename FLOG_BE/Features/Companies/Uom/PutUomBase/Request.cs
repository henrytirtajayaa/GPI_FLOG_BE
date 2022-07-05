using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Uom.PutUomBase
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestUpdate Body { get; set; }
    }

    public class RequestUpdate
    {
        public Guid UomBaseId { get; set; }
        public string UomCode { get; set; }
        public string UomName { get; set; }        
    }
}
