using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOG_BE.Model.Companies.Entities
{
    [Table("company_branch", Schema = "dbo")]
    public class CompanyBranch
    {
        public CompanyBranch()
        {
            #region Constructor
            #endregion
        }
        #region Properties

        [Key]
        [Column("company_branch_id", TypeName = "uniqueidentifier")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(50)]        
        public Guid CompanyBranchId { get; set; }

        [Column("branch_code", TypeName = "varchar(50)")]
        [Required]
        [MaxLength(50)]
        public string BranchCode { get; set; }

        [Column("branch_name", TypeName = "varchar(250)")]
        [MaxLength(250)]
        public string BranchName { get; set; }

        [Column("default", TypeName = "bit")]
        public bool Default { get; set; }

        [Column("created_by", TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        [Column("created_date", TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        [Column("modified_by", TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [Column("modified_date", TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }

        [Column("inactive", TypeName = "bit")]
        public bool Inactive { get; set; }

        #endregion

    }
}
