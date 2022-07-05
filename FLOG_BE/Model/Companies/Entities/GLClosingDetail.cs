using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    [Table("gl_closing_detail")]
    public class GLClosingDetail
    {
        public GLClosingDetail()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        [Key]
        [Column("closingdetail_id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ClosingDetailId { get; set; }
        [Column("closingheader_id", TypeName = "bigint")]
        public long ClosingHeaderId { get; set; }
        [Column("account_id", TypeName = "varchar(50)")]
        public string AccountId { get; set; }
        [Column("branch_code", TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string BranchCode { get; set; }
        [Column("debit", TypeName = "decimal(20,5)")]
        public decimal Debit { get; set; }
        [Column("credit", TypeName = "decimal(20,5)")]
        public decimal Credit { get; set; }
        [Column("balance", TypeName = "decimal(20,5)")]
        public decimal Balance { get; set; }
        [Column("status", TypeName = "int")]
        public int Status { get; set; }
        #endregion
    }
}
