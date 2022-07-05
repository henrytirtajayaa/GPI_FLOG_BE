using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    [Table("gl_statement_subcategory")]
    public class GLStatementSubCategory
    {
        public GLStatementSubCategory()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        [Key]
        [Column("subcategory_id", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubCategoryId { get; set; }
        [Column("category_id", TypeName = "int")]
        public int CategoryId { get; set; }
        [Column("subcategory_key", TypeName = "varchar(100)")]
        public string SubCategoryKey { get; set; }
        [Column("subcategory_caption", TypeName = "varchar(250)")]
        public string SubCategoryCaption { get; set; }
        [Column("is_param_total", TypeName = "bit")]
        public bool IsParamTotal { get; set; }
        [Column("inflow", TypeName = "bit")]
        public bool Inflow { get; set; }
        [Column("pos_index", TypeName = "int")]
        public int PosIndex { get; set; }

        [NotMapped]
        public string CategoryKey { get; set; }
        [NotMapped]
        public string CategoryCaption { get; set; }
        [NotMapped]
        public int StatementType { get; set; }

        #endregion
    }
}
