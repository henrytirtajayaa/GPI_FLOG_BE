using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Finance.Payable.GetCreditNote
{
    public class Response
    {
        public List<Model.Companies.View.APCreditNote> CreditNotes { get; set; }
        public ListInfo ListInfo { get; set; }
    }

}
