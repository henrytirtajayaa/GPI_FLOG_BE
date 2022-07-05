using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Finance.Receivable.GetUnapplyReceipt
{
    public class Response
    {
        public List<Model.Companies.View.ARUnapplyReceipt> Unapplies { get; set; }
        public ListInfo ListInfo { get; set; }
    }

}
