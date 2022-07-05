using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class SalesPerson
    {
        public SalesPerson()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid SalesPersonId { get; set; }
        public string SalesCode { get; set; }
        public string SalesName { get; set; }
        public Guid PersonId { get; set; }

        [NotMapped]
        public string PersonFullName { get; set; }
        #endregion

        #region Generated Relationship
        #endregion
    }
}
