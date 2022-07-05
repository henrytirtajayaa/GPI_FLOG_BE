using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    [Table("fn_document_number_setup")]
    public class FNDocNumberSetup
    {
        public FNDocNumberSetup()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        [Key]
        [Column("doc_number_id", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocNumberId { get; set; }

        [Column("trx_module", TypeName = "int")]
        [Required]
        public int TrxModule { get; set; }

        [Column("transaction_type", TypeName = "varchar(50)")]
        public string TransactionType { get; set; }

        [Column("doc_feature_id", TypeName = "int")]
        public int DocFeatureId { get; set; }

        [Column("doc_no", TypeName = "varchar(50)")]
        public string DocNo { get; set; }

        [Column("description", TypeName = "varchar(250)")]
        public string Description { get; set; }

        [NotMapped]
        public string TrxModuleCaption { get; set; }

        #endregion
    }
}
