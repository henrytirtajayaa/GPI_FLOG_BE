using FLOG_BE.Model.Companies.Entities;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Model.Central.Entities
{
    public partial class Person
    {
        public Person()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public string PersonId { get; set; }

        public string PersonFullName { get; set; }

        public string PersonPassword { get; set; }

        public string EmailAddress { get; set; }

        public string PersonCategoryId { get; set; }
      
        public bool InActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Generated Relationships
        public PersonCategory PersonCategory { get; set; }
        #endregion

    }
}
