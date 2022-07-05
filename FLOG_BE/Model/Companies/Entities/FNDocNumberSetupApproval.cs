using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    [Table("fn_document_number_setup_approval")]
    public class FNDocNumberSetupApproval
    {
        public FNDocNumberSetupApproval()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        [Key]
        [Column("fn_docnumber_setup_approval_id", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocNumberSetupApprovalId { get; set; }

        [Column("trx_module", TypeName = "int")]
        [Required]
        public int TrxModule { get; set; }

        [Column("transaction_type", TypeName = "varchar(50)")]
        public string TransactionType { get; set; }

        [Column("doc_feature_id", TypeName = "int")]
        public int DocFeatureId { get; set; }

        [Column("mode_status", TypeName = "int")]
        public int ModeStatus { get; set; }

        [Column("approval_code", TypeName = "varchar(50)")]
        public string ApprovalCode { get; set; }

        [Column("modified_by", TypeName = "varchar(50)")]
        public string ModifiedBy { get; set; }

        [Column("modified_date", TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }

        [NotMapped]
        public string TrxModuleCaption { get; set; }

        #endregion
    }
}
