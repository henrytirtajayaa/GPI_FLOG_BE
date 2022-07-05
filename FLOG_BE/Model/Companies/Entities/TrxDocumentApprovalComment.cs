using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class TrxDocumentApprovalComment
    {
        public TrxDocumentApprovalComment()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        public Guid TrxDocumentApprovalCommentId { get; set; }
        public Guid TrxDocumentApprovalId { get; set; }
        public int Status { get; set; }
        public Guid PersonId { get; set; }
        public DateTime CommentDate { get; set; }
        public string Comments { get; set; }

        [NotMapped]
        public string StatusCaption { get; set; }

        [NotMapped]
        public string UserFullName { get; set; }

        [NotMapped]
        public Guid TransactionId { get; set; }

        #endregion


    }
}
