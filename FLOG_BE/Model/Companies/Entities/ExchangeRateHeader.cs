using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class ExchangeRateHeader
    {
        public ExchangeRateHeader()
        {
            #region Generated Constructor
            #endregion
        }
        #region Generated Properties

        public Guid ExchangeRateHeaderId { get; set; }
        public string ExchangeRateCode { get; set; }
        public string Description { get; set; }
        public string CurrencyCode { get; set; }
        public int RateType { get; set; }
        public string ExpiredPeriod { get; set; }
        public int CalculationType { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion

    }
}
