using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class ApPaymentDetail
    {
        public ApPaymentDetail()
        {
            #region Generated Constructor
            #endregion
        }
        #region Generated Properties
        public Guid PaymentDetailId { get; set; }
        public Int64 RowId { get; set; }
        public Guid PaymentHeaderId { get; set; }
        public Guid PayableTransactionId { get; set; }
        public Guid PayableApplyDetailId { get; set; }
        [NotMapped]
        public string DocumentNo { get; set; } 
        [NotMapped]
        public string VendorCode { get; set; }
        [NotMapped]
        public string NsDocumentNo { get; set; }
        [NotMapped]
        public string MasterNo { get; set; }
        [NotMapped]
        public string AgreementNo { get; set; }
        [NotMapped]
        public decimal OrgDocAmount { get; set; }
        public string Description { get; set; }
        public decimal OriginatingBalance { get; set; }
        public decimal FunctionalBalance { get; set; }
        public decimal OriginatingPaid { get; set; }
        public decimal FunctionalPaid { get; set; }
        public int Status { get; set; }
        #endregion
    }
}
