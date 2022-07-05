using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class MSTransactionType
    {
        public MSTransactionType()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid TransactionTypeId { get; set; }
        public string TransactionType { get; set; }
        public string TransactionName { get; set; }
        public int PaymentCondition { get; set; }
        public int RequiredSubject { get; set; }
        public bool InActive { get; set; }

        #endregion

    }
}
