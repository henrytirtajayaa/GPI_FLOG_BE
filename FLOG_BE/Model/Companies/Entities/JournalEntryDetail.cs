using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOG_BE.Model.Companies.Entities
{
    [Table("journal_entry_detail", Schema = "dbo")]
    public class JournalEntryDetail
    {
        public JournalEntryDetail()
        {
            #region Constructor
            #endregion
        }
        #region Properties

        [Key]
        [Column("journal_entry_detail_id", TypeName = "uniqueidentifier")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(50)]
        public Guid JournalEntryDetailId { get; set; }

        [Column("journal_entry_header_id", TypeName = "uniqueidentifier")]
        [Required]
        [MaxLength(50)]
        public Guid JournalEntryHeaderId { get; set; }

        [Column("account_id", TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string AccountId { get; set; }

        [Column("description", TypeName = "varchar(255)")]
        [MaxLength(255)]
        public string Description { get; set; }

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

        [Column("row_index", TypeName = "integer")]
        public int RowIndex { get; set; }

        public Int64 RowId { get; set; }
        
        [NotMapped]
        public string AccountDescription { get; set; }
        #endregion

    }
}
