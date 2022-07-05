using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class CheckbookTransactionDetail
    {
        public CheckbookTransactionDetail()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid TransactionDetailId { get; set; }
        public Guid CheckbookTransactionId { get; set; }
        public Guid ChargesId { get; set; }
        public string ChargesDescription { get; set; }
        public decimal OriginatingAmount { get; set; }
        public decimal FunctionalAmount { get; set; }
        public int Status { get; set; }
        public int RowIndex { get; set; }
        #endregion


    }
}
