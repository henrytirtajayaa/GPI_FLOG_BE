using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class ApPaymentApproval
    {
        public ApPaymentApproval()
        {
            #region Generated Constructor
            #endregion
        }
        #region Generated Properties
        public Guid PaymentApprovalId { get; set; }
        public Guid PaymentHeaderId { get; set; }
     
        public int Index { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? PersonCategoryId { get; set; }
        public int Status { get; set; }
        public Int64 RowId { get; set; }
        [IgnoreDataMember]
        public virtual List<ApPaymentApprovalComment> ApPaymentApprovalComments { get; set; }
        #endregion
    }
}
