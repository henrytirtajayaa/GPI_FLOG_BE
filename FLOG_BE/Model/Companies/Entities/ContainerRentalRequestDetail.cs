using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class ContainerRentalRequestDetail
    {
        public ContainerRentalRequestDetail()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid ContainerRentalRequestDetailId { get; set; }
        public Guid ContainerRentalRequestHeaderId { get; set; }
        public string ContainerCode { get; set; }
        public string ContainerName { get; set; }
        public string UomCode { get; set; }
        public string Remarks { get; set; }
        public int Quantity { get; set; }
        #endregion
    }
}
