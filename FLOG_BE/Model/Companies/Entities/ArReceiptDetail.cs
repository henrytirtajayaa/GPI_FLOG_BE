using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class ArReceiptDetail
    {
        public ArReceiptDetail()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid ReceiptDetailId { get; set; }
        public Int64 RowId { get; set; }
        public Guid ReceiptHeaderId { get; set; }
        public Guid ReceivableApplyDetailId { get; set; }
        [NotMapped]
        public string DocumentNo { get; set; }
        [NotMapped]
        public string CustomerName { get; set; } 
        [NotMapped]
        public string NsDocumentNo { get; set; }
        [NotMapped]
        public decimal OrgDocAmount { get; set; }
        [NotMapped]
        public string MasterNo { get; set; }
        [NotMapped]
        public string AgreementNo { get; set; }
        public Guid ReceiveTransactionId { get; set; }
        public string Description { get; set; }
        public decimal OriginatingBalance { get; set; }
        public decimal FunctionalBalance { get; set; }
        public decimal OriginatingPaid { get; set; }
        public decimal FunctionalPaid { get; set; }
        public int Status { get; set; }
        #endregion
    }
}
