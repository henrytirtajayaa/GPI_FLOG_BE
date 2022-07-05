using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOG_BE.Model.Companies.Entities
{
    [Table("distribution_journal_header", Schema = "dbo")]
    public class DistributionJournalHeader
    {
        public DistributionJournalHeader()
        {
            #region Constructor
            #endregion
        }
        #region Properties

        [Key]
        [Column("distribution_header_id", TypeName = "uniqueidentifier")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [MaxLength(50)]        
        public Guid DistributionHeaderId { get; set; }

        [Column("trx_module", TypeName = "integer")]
        [Required]
        public int TrxModule { get; set; }

        [Column("transaction_id", TypeName = "uniqueidentifier")]
        [Required]
        [MaxLength(50)]
        public Guid TransactionId { get; set; }

        [Column("document_no", TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string DocumentNo { get; set; }

        [Column("document_date", TypeName = "date")]
        public DateTime DocumentDate { get; set; }

        [Column("currency_code", TypeName = "varchar(50)")]
        public string CurrencyCode { get; set; }

        [Column("exchange_rate", TypeName = "decimal(10,5)")]
        public decimal ExchangeRate { get; set; }

        [Column("is_multiply", TypeName = "bit")]
        public bool IsMultiply { get; set; }


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

        [Column("status", TypeName = "integer")]
        public int Status { get; set; }

        [NotMapped]
        public int CurrencyDecimalPoint { get; set; }

        [NotMapped]
        public List<DistributionJournalDetail> DistributionJournalDetails { get; set; }

        #endregion

    }
}
