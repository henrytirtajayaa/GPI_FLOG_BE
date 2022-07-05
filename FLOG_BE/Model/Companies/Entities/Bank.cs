using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class Bank
    {
        public Bank()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        public Guid BankId { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string Address { get; set; }
        public string CityCode { get; set; }
        public bool InActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion

        #region Generated Relationships    
        public City Cities { get; set; }
        #endregion

    }
}
