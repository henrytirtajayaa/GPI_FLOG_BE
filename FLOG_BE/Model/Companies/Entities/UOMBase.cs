using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    [Table("uom_base")]
    public class UOMBase
    {
        public UOMBase()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        [Key]
        [Required]
        [Column("uom_base_id", TypeName = "uniqueidentifier")]
        public Guid UomBaseId { get; set; }

        [Column("uom_code", TypeName = "varchar(50)")]
        [Required]
        [MaxLength(50)]
        public string UomCode { get; set; }

        [Column("uom_name", TypeName = "varchar(250)")]
        [MaxLength(250)]
        public string UomName { get; set; }
        
        #endregion
    }
}
