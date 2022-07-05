using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class CheckbookTransactionApproval
    {
        public CheckbookTransactionApproval()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid CheckbookTransactionApprovalId { get; set; }
        public Guid CheckbookTransactionId { get; set; }
        public int Index { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? PersonCategoryId { get; set; }
        public int Status { get; set; }
        #endregion


    }
}
