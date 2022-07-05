using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Companies.Entities;

namespace FLOG_BE.Features.Sales.NegotiationSheet.PostNegotiationSheet
{
    public class Response
    {
        public Guid NegotiationSheetId { get; set; }
        public Int64 RowId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string DocumentNo { get; set; }

    }
}
