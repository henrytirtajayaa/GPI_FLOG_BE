using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class PayableSetup
    {
        public PayableSetup()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid PayableSetupId { get; set; }
        public int DefaultRateType { get; set; }
        public int TaxRateType { get; set; }
        public bool AgingByDocdate { get; set; }
        public decimal WriteoffLimit { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion

        #region Generated Relationship
        #endregion
    }
}
