using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class CheckbookApprovalComment
    {
        public CheckbookApprovalComment()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid ApprovalCommentId { get; set; }
        public Guid CheckbookTransactionApprovalId { get; set; }
        public Guid PersonId { get; set; }
        public DateTime CommentDate { get; set; }
        public string Comments { get; set; }
        public int Status { get; set; }
        #endregion


    }
}
