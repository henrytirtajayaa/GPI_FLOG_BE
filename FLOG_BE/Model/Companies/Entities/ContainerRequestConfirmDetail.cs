using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class ContainerRequestConfirmDetail
    {
        public ContainerRequestConfirmDetail()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid ContainerRequestConfirmDetailId { get; set; }
        public Guid ContainerRequestConfirmHeaderId { get; set; }
        public Guid ContainerRentalRequestDetailId { get; set; }
        [NotMapped] public string ContainerCode { get; set; }
        [NotMapped] public string ContainerName { get; set; }
        [NotMapped] public string UomCode { get; set; }
        public string Remarks { get; set; }
        public string Grade { get; set; }
        [NotMapped] public int Quantity { get; set; }
        public int QuantityToConfirm { get; set; }
        public int QuantityBalance { get; set; }

        [NotMapped] public int QuantityRemaining { get; set; }
        #endregion
    }
}
