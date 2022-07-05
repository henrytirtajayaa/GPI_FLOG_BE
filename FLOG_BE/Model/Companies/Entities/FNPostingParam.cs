using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    [Table("fn_posting_parameter")]
    public class FNPostingParam
    {
        public FNPostingParam()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        [Key]
        [Column("param_id", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParamId { get; set; }
        [Column("posting_key", TypeName = "int")]
        public int PostingKey { get; set; }
        [Column("account_id", TypeName = "varchar(50)")]
        public string AccountId { get; set; }
        [Column("description", TypeName = "varchar(250)")]
        public string Description { get; set; }
       
        #endregion
    }
}
