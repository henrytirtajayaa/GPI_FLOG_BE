using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Utils;

namespace FLOG_BE.Features.Companies.PayableSetup.GetPayableSetup
{
    public class Response
    {
        public List<ResponseItem> PayableSetups { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid PayableSetupId { get; set; }
        public int DefaultRateType { get; set; }
        public int TaxRateType { get; set; }
        public bool AgingByDocdate { get; set; }
        public decimal WriteoffLimit { get; set; }
    }
}
