using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class ApPaymentApprovalComment
    {
        public ApPaymentApprovalComment()
        {
            #region Generated Constructor
            #endregion
        }
     
        #region Generated Properties
        public Guid PaymentApprovalCommentId { get; set; }
        public Guid PaymentApprovalId { get; set; }
        public int Status { get; set; }
        public Guid PersonId { get; set; }
        public DateTime CommentDate { get; set; }
        public string Comments{ get; set; }
        [NotMapped]
        public string UserFullName { get; set; }
        public Int64 RowId { get; set; }
        public ApPaymentApproval ApPaymentApprovals { get; set; }
       
        #endregion
    }
}
