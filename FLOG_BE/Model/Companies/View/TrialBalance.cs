using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.View 
{
    //FUNCTION : [dbo].fxnGLTrialBalance(@branchCode varchar)
    public class TrialBalance
    {
        [Column("PostedYear")]
        public int PostedYear { get; set; }
        [Column("PostedMonth")]
        public int PostedMonth { get; set; }
        [Column("AccountId", TypeName = "varchar")]
        public string AccountId { get; set; }
        [Column("Description", TypeName = "varchar")]
        public string Description { get; set; }
        [Column("PrevBalance", TypeName = "decimal")]
        public decimal PrevBalance { get; set; }
        [Column("Debit", TypeName = "decimal")]
        public decimal Debit { get; set; }
        [Column("Credit", TypeName = "decimal")]
        public decimal Credit { get; set; }
        [Column("Balance", TypeName = "decimal")]
        public decimal Balance { get; set; }
        
    }
}
