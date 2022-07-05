using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class FiscalPeriodDetail
    {
        public FiscalPeriodDetail()
        {
            #region Generated Constructor
            #endregion
        }
        #region Generated Properties

        public Guid FiscalDetailId { get; set; }
        public Guid FiscalHeaderId { get; set; }
        public int PeriodIndex { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public bool IsClosePurchasing { get; set; }
        public bool IsCloseSales { get; set; }
        public bool IsCloseInventory { get; set; }
        public bool IsCloseFinancial { get; set; }
        public bool IsCloseAsset { get; set; }
        #endregion

    }
}
