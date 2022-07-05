using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Features.Companies.NumberFormatHeader.PostNumberFormatHeader;
using Infrastructure.Mediator;

namespace FLOG_BE.Features.Companies.NumberFormatHeader.PutNumberFormatHeader
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestBodyUpdateNumberFormatHeader Body { get; set; }

    }

    public class RequestBodyUpdateNumberFormatHeader
    {
        public string FormatHeaderId { get; set; }
        public string DocumentId { get; set; }
        public string Description { get; set; }
        public string LastGeneratedNo { get; set; }
        public string NumberFormat { get; set; }
        public bool InActive { get; set; }
        public bool IsMonthlyReset { get; set; }
        public bool IsYearlyReset { get; set; }
        public List<RequestBodyPostNumberFormatDetail> NumberFormatDetails { get; set; }
    }
}
