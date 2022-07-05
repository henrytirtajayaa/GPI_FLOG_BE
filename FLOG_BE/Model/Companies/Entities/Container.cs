using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class Container
    {
        public Container()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid ContainerId { get; set; }
        public string ContainerCode { get; set; }
        public string ContainerName { get; set; }
        public int ContainerSize { get; set; }
        public string ContainerType { get; set; }
        public int ContainerTeus { get; set; }
        public bool Inactive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion


    }
}
