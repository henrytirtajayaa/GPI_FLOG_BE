using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    [Table("uom_header")]
    public class UOMHeader
    {
        public UOMHeader()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        [Key]
        [Required]
        [Column("uom_header_id", TypeName = "uniqueidentifier")]
        public Guid UomHeaderId { get; set; }

        [Column("uom_schedule_code", TypeName = "varchar(50)")]
        [Required]
        [MaxLength(50)]
        public string UomScheduleCode { get; set; }

        [Column("uom_schedule_name", TypeName = "varchar(250)")]
        [MaxLength(250)]
        public string UomScheduleName { get; set; }

        [Column("uom_base_id", TypeName = "uniqueidentifier")]
        public Guid UomBaseId { get; set; }

        [Column("modified_by", TypeName = "varchar(50)")]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [Column("modified_date", TypeName = "datetime")]
        [MaxLength(250)]
        public DateTime ModifiedDate { get; set; }

        [Column("inactive", TypeName = "bit")]
        public bool Inactive { get; set; }

        [NotMapped]
        public string UomBaseCode { get; set; }
        [NotMapped]
        public string UomBaseName { get; set; }
        [NotMapped]
        public List<UOMDetail> UomDetails { get; set; }

        #endregion
    }
}
