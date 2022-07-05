using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Central.Entities;
using Entities = FLOG_BE.Model.Companies.Entities;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Sales.NegotiationSheet.GetNegotiationSheetProgress
{
    public class Response
    {
        public List<Entities.NegotiationSheetHeader> NegotiationSheetHeaders { get; set; }
        public ListInfo ListInfo { get; set; }
    }

}
