using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    [Table("gl_statement_category")]
    public class GLStatementCategory
    {
        public GLStatementCategory()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        [Key]
        [Column("category_id", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [Column("statement_type", TypeName = "int")]
        public int StatementType { get; set; }
        [Column("category_key")]
        public string CategoryKey { get; set; }
        [Column("category_caption")]
        public string CategoryCaption { get; set; }
        
        [NotMapped]
        public List<GLStatementSubCategory> SubCategories { get; set; }

        #endregion
    }
}
