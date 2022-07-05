using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class TrxDocumentApproval
    {
        public TrxDocumentApproval()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        public Guid TrxDocumentApprovalId { get; set; }
        public int TrxModule { get; set; }
        public string TransactionType { get; set; }
        public int DocFeatureId { get; set; }
        public int ModeStatus { get; set; }
        public Guid TransactionId { get; set; }        
        public int Index { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? PersonCategoryId { get; set; }
        public int Status { get; set; }

        #endregion


    }
}
