using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class ChargesDetail
    {
        public ChargesDetail()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid ChargesDetailId { get; set; }
        public Guid ChargesId { get; set; }
        public int TrxModule { get; set; }
        public Guid ShippingLineId { get; set; }
        public string RevenueAccountNo { get; set; }
        public string TempRevenueAccountNo { get; set; }
        public string CostAccountNo { get; set; }    
        
        #endregion

        [NotMapped]
        public string ShippingLineCode { get; set; }
        [NotMapped]
        public string ShippingLineName { get; set; }
    }
}
