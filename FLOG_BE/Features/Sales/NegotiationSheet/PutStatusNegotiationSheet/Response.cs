using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Sales.NegotiationSheet.PutStatusNegotiationSheet
{
    public class Response
    {
        public Guid NegotiationSheetId { get; set; }
        public string Message { get; set; }
    }


}
