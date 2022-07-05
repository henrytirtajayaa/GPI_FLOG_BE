using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOG_BE.Features.Finance.GLClosing.PostGLClosingMonth
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestFormBody Body { get; set; }
    }

    public class RequestFormBody
    {
        public string CurrencyCode { get; set; }
        public int PeriodYear { get; set; }
        public int PeriodIndex { get; set; }

    }

}
