using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    [Table("gl_statement_detail")]
    public class GLStatementDetail
    {
        public GLStatementDetail()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        [Key]
        [Column("detail_id", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailId { get; set; }
        [Column("subcategory_id", TypeName = "int")]
        public int SubCategoryId { get; set; }
        [Column("account_name")]
        public string AccountName { get; set; }
        [Column("pos_index", TypeName = "int")]
        public int PosIndex { get; set; }
        [Column("is_cashflow", TypeName = "bit")]
        public bool IsCashflow { get; set; }
        [Column("show_zero_value", TypeName = "bit")]
        public bool ShowZeroValue { get; set; }
        [Column("is_cashflow_dynamic", TypeName = "bit")]
        public bool IsCashflowDynamic { get; set; }

        [NotMapped]
        public string SubCategoryKey { get; set; }
        [NotMapped]
        public string SubCategoryCaption { get; set; }
        [NotMapped]
        public int CategoryId { get; set; }
        [NotMapped]
        public string CategoryKey { get; set; }
        [NotMapped]
        public string CategoryCaption { get; set; }
        [NotMapped]
        public List<Account> DetailAccounts { get; set; }
        #endregion
    }
}
