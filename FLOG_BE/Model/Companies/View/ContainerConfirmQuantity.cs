using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.View
{
    // VIEW : vw_container_confirm_quantity
    public class ContainerConfirmQuantity
    {
        [Column("ContainerRentalRequestHeaderId", TypeName  = "uniqueidentifier")]
        public Guid ContainerRentalRequestHeaderId { get; set; }

        [Column("ContainerRentalRequestDetailId", TypeName = "uniqueidentifier")]
        public Guid ContainerRentalRequestDetailId { get; set; }

        [Column("ContainerRequestConfirmHeaderId", TypeName = "uniqueidentifier")]
        public Guid ContainerRequestConfirmHeaderId { get; set; }

        [Column("ContainerRequestConfirmDetailId", TypeName = "uniqueidentifier")]
        public Guid ContainerRequestConfirmDetailId { get; set; }

        [Column("ContainerCode", TypeName = "varchar(50)")]
        public string ContainerCode { get; set; }

        [Column("ContainerName", TypeName = "varchar(250)")]
        public string ContainerName { get; set; }

        [Column("UomCode", TypeName = "varchar(50)")]
        public string UomCode { get; set; }

       // [Column("Grade", TypeName = "varchar(50)")]
       // public string Grade { get; set; }

       // [Column("Remarks", TypeName = "varchar(250)")]
       // public string Remarks { get; set; }

        [Column("Quantity", TypeName = "int")]
        public int Quantity { get; set; }

        [Column("QuantityToConfirm", TypeName = "int")]
        public int QuantityToConfirm { get; set; }

        [Column("QuantityRemaining", TypeName = "int")]
        public int QuantityRemaining { get; set; }
    }
}
