using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class SalesOrderContainer
    {
        public SalesOrderContainer()
        {
            #region Generated Constructor
            #endregion
        }
        #region Generated Properties

        public Guid SalesOrderContainerId { get; set; }
        public Int64 RowId { get; set; }
        public Guid SalesOrderId { get; set; }
        public Guid ContainerId { get; set; }
        [NotMapped]
        public string ContainerCode { get; set; }
        [NotMapped]
        public string ContainerName { get; set; }
        public int Qty { get; set; }
        public Guid UomDetailId { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
        public int RowIndex { get; set; }

        [NotMapped]
        public SalesOrderHeader SalesOrderHeaders { get; set; }
        #endregion

    }
}
