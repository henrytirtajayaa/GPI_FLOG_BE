using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOG_BE.Model.Companies.Entities
{
    [Table("distribution_journal_detail", Schema = "dbo")]
    public class DistributionJournalDetail
    {
        public DistributionJournalDetail()
        {
            #region Constructor
            #endregion
        }
        #region Properties

        [Key]
        [Column("distribution_detail_id", TypeName = "uniqueidentifier")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(50)]
        public Guid DistributionDetailId { get; set; }

        [Column("distribution_header_id", TypeName = "uniqueidentifier")]
        [Required]
        [MaxLength(50)]
        public Guid DistributionHeaderId { get; set; }

        [Column("account_id", TypeName = "varchar(50)")]
        [Required]
        [MaxLength(50)]
        public string AccountId { get; set; }

        [Column("branch_code", TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string BranchCode { get; set; }

        [Column("charges_id", TypeName = "uniqueidentifier")]
        [MaxLength(50)]
        public Guid ChargesId { get; set; }

        [Column("journal_date", TypeName = "date")]
        public DateTime JournalDate { get; set; }

        [Column("journal_note", TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string JournalNote { get; set; }

        [Column("originating_debit", TypeName = "decimal(20,5)")]
        public decimal OriginatingDebit { get; set; }

        [Column("functional_debit", TypeName = "decimal(20,5)")]
        public decimal FunctionalDebit { get; set; }

        [Column("originating_credit", TypeName = "decimal(20,5)")]
        public decimal OriginatingCredit { get; set; }

        [Column("functional_credit", TypeName = "decimal(20,5)")]
        public decimal FunctionalCredit { get; set; }

        [Column("status", TypeName = "integer")]
        public int Status { get; set; }

        [Column("modified_by", TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [Column("modified_date", TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }

        [NotMapped]
        public string AccountDesc { get; set; }

        #endregion

    }
}
