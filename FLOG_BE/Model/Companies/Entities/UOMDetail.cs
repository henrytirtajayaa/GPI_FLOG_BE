using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    [Table("uom_detail")]
    public class UOMDetail
    {
        public UOMDetail()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        [Key]
        [Required]
        [Column("uom_detail_id", TypeName = "uniqueidentifier")]
        public Guid UomDetailId { get; set; }

        [Required]
        [Column("uom_header_id", TypeName = "uniqueidentifier")]
        public Guid UomHeaderId { get; set; }

        [Column("uom_code", TypeName = "varchar(50)")]
        [Required]
        [MaxLength(50)]
        public string UomCode { get; set; }

        [Column("uom_name", TypeName = "varchar(250)")]
        [MaxLength(250)]
        public string UomName { get; set; }

        [Column("eq_qty", TypeName = "decimal(20,5)")]
        public decimal EquivalentQty { get; set; }

        #endregion
    }
}
