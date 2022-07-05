using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    [Table("gl_closing_header")]
    public class GLClosingHeader
    {
        public GLClosingHeader()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        [Key]
        [Column("closingheader_id", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ClosingHeaderId { get; set; }
        [Column("closing_date", TypeName = "datetime")]
        public DateTime ClosingDate { get; set; }
        [Column("period_year", TypeName = "int")]
        public int PeriodYear { get; set; }
        [Column("period_index", TypeName = "int")]
        public int PeriodIndex { get; set; }
        [Column("currency_code", TypeName = "varchar(50)")]
        public string CurrencyCode { get; set; }
        [Column("account_id", TypeName = "varchar(50)")]
        public string AccountId { get; set; }
        [Column("retain_balance", TypeName = "decimal(20,5)")]
        public decimal RetainBalance { get; set; }
        [Column("is_yearly", TypeName = "bit")]
        public bool IsYearly { get; set; }
        [Column("closed_by", TypeName = "varchar(50)")]
        public string ClosedBy { get; set; }
        [Column("unclosed_by", TypeName = "varchar(50)")]
        public string UnclosedBy { get; set; }
        [Column("unclosed_date", TypeName = "datetime")]
        public DateTime? UnclosedDate { get; set; }
        [Column("status", TypeName = "int")]
        public int Status { get; set; }
       
        #endregion
    }
}
