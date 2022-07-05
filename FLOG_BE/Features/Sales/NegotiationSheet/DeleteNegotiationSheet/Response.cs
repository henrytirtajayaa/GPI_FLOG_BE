using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Features.Sales.NegotiationSheet.DeleteNegotiationSheet
{
    public class Response
    {
        public Guid NegotiationSheetId { get; set; }
        public String Message { get; set; }
    }
}
