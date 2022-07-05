using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FLOG_BE.Model.Central.Entities
{
    public partial class CompanySecurity
    {
        public CompanySecurity()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public string CompanySecurityId { get; set; }

        public string PersonId { get; set; }

        public string CompanyId { get; set; }

        public string SecurityRoleId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Generated Relationships
        [IgnoreDataMember]
        public virtual Company Company { get; set; }
        [IgnoreDataMember]
        public virtual SecurityRole SecurityRole { get; set; }
        [IgnoreDataMember]
        public virtual Person Person { get; set; }
        #endregion

    }
}
