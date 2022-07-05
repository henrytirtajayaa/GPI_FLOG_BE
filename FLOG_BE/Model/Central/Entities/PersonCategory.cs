using System;
using System.Collections.Generic;

namespace FLOG_BE.Model.Central.Entities
{
    public partial class PersonCategory
    {
        public PersonCategory()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public string PersonCategoryId { get; set; }

        public string PersonCategoryCode { get; set; }

        public string PersonCategoryName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        #endregion

        #region Generated Relationships
        #endregion

    }
}
