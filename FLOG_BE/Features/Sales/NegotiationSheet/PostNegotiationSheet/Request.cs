using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;
using System.ComponentModel.DataAnnotations.Schema;
using PutNegotiationSheet = FLOG_BE.Features.Sales.NegotiationSheet.PutNegotiationSheet;

namespace FLOG_BE.Features.Sales.NegotiationSheet.PostNegotiationSheet
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }

        public PutNegotiationSheet.RequestNegotiationSheetBody Body { get; set; }
    }

}
