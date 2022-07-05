using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class ExchangeRateDetail
    {
        public ExchangeRateDetail()
        {
            #region Generated Constructor
            #endregion
        }
        #region Generated Properties

        public Guid ExchangeRateDetailId { get; set; }
        public Guid ExchangeRateHeaderId { get; set; }
        public DateTime RateDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public decimal RateAmount { get; set; }
        public int Status { get; set; }
        #endregion

    }
}
