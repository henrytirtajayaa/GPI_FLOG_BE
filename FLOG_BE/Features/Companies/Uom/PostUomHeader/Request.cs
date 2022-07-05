using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.Uom.PostUomHeader
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestUomBody Body { get; set; }
    }

    public class RequestUomBody
    {
        public string UomScheduleCode { get; set; }
        public string UomScheduleName { get; set; }
        public Guid UomBaseId { get; set; }
        public List<RequestUomDetail> UomDetails { get; set; }
    }

    public class RequestUomDetail
    {
        public string UomCode { get; set; }
        public string UomName { get; set; }
        public decimal EquivalentQty { get; set; }
    }
}
