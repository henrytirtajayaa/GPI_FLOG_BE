using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class SalesQuotationDetail
    {
        public SalesQuotationDetail()
        {
            #region Generated Constructor
            #endregion
        }
        #region Generated Properties
     
        public Guid SalesQuotationDetailId { get; set; }
        public Int64 RowId { get; set; }
        public Guid SalesQuotationId { get; set; }
        public Guid ContainerId { get; set; }
        [NotMapped]
        public string ContainerCode { get; set; }
        [NotMapped]
        public string ContainerName { get; set; }
        public int Qty { get; set; }
        public Guid UomDetailId { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public int RowIndex { get; set; }

        [NotMapped]
        public SalesQuotationHeader SalesQuotationHeaders { get; set; }
        #endregion

    }
}
