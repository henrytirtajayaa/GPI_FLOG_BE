using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOG_BE.Model.Companies.Entities
{
    [Table("journal_entry_header", Schema = "dbo")]
    public class JournalEntryHeader
    {
        public JournalEntryHeader()
        {
            #region Constructor
            #endregion
        }
        #region Properties

        [Key]
        [Column("journal_entry_header_id", TypeName = "uniqueidentifier")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(50)]        
        public Guid JournalEntryHeaderId { get; set; }

        public Int64 RowId { get; set; }

        [Column("document_no", TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string DocumentNo { get; set; }

        [Column("transaction_date", TypeName = "date")]
        public DateTime TransactionDate { get; set; }

        [Column("branch_code", TypeName = "varchar(50)")]
        public string BranchCode { get; set; }

        [Column("currency_code", TypeName = "varchar(50)")]
        public string CurrencyCode { get; set; }

        [Column("exchange_rate", TypeName = "decimal(20,5)")]
        public decimal ExchangeRate { get; set; }

        [Column("is_multiply", TypeName = "bit")]
        public bool IsMultiply { get; set; }

        [Column("source_document", TypeName = "varchar(100)")]
        public string SourceDocument { get; set; }

        [Column("description", TypeName = "text")]
        public string Description { get; set; }

        [Column("originating_total", TypeName = "decimal(20,5)")]
        public decimal OriginatingTotal { get; set; }

        [Column("functional_total", TypeName = "decimal(20,5)")]
        public decimal FunctionalTotal { get; set; }

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

        [Column("void_by", TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string VoidBy { get; set; }

        [Column("void_date", TypeName = "datetime")]
        public DateTime? VoidDate { get; set; }

        [Column("status", TypeName = "integer")]
        public int Status { get; set; }

        [Column("status_comment", TypeName = "varchar(255)")]
        public string StatusComment { get; set; }

        [NotMapped]
        public string CreatedByName { get; set; }

        [NotMapped]
        public string ModifiedByName { get; set; }

        [NotMapped]
        public List<JournalEntryDetail> JournalEntryDetails { get; set; }

        [NotMapped]
        public int DecimalPlaces { get; set; }

        #endregion

    }
}
