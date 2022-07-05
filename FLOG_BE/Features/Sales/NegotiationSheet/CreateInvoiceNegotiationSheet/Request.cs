using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Mediator;
using Entities = FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Sales.NegotiationSheet.CreateInvoiceNegotiationSheet
{
    public class Request : IRequest<Response>
    {
        public UserLogin Initiator { get; set; }
        public RequestPutStatus Body { get; set; }
    }

    public class RequestPutStatus
    {
        public Guid NegotiationSheetId { get; set; }

        public List<Entities.NegotiationSheetSelling> NsSellings { get; set; }
        public List<Entities.NegotiationSheetBuying> NsBuyings { get; set; }

        public DateTime InvoiceDate { get; set; }
        public int ActionDocStatus { get; set; }
        public string Comments { get; set; }
        
    }

}
