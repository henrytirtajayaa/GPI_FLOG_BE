using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOG_BE.Features.Companies.ReceivableSetup.GetReceivableSetup
{
    public class Response
    {
        public List<ResponseItem> ReceivableSetups { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public string ReceivableSetupId { get; set; }
        public string TransactionType { get; set; }
        public int DefaultRateType { get; set; }
        public int TaxRateType { get; set; }
        public bool AgingByDocdate { get; set; }
        public decimal WriteoffLimit { get; set; }        
    }
}
