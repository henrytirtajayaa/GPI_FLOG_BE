using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    [Table("gl_statement_detail_sub")]
    public class GLStatementDetailSub
    {
        public GLStatementDetailSub()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        [Key]
        [Column("subdetail_id", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubDetailId { get; set; }
        [Column("detail_id", TypeName = "int")]
        public int DetailId { get; set; }
        [Column("account_id")]
        public string AccountId { get; set; }
        
        [NotMapped]
        public string AccountName { get; set; }
        [NotMapped]
        public string AccountDescription { get; set; }

        #endregion
    }
}
