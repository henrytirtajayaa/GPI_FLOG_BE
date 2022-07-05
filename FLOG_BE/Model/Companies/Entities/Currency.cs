using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class Currency
    {
        public Currency()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public string Symbol { get; set; }
        public int DecimalPlaces { get; set; }
        public string RealizedGainAcc { get; set; }
        public string RealizedLossAcc { get; set; }
        public string UnrealizedGainAcc { get; set; }
        public string UnrealizedLossAcc { get; set; }
        public bool Inactive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CurrencyUnit { get; set; }
        public string CurrencySubUnit { get; set; }
        [NotMapped]
        public bool IsFunctional { get; set; }
        #endregion


    }
}
