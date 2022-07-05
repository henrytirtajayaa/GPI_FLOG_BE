using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FLOG_BE.Model.Central.Entities
{
    public partial class SecurityRole
    {
        public SecurityRole()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public string SecurityRoleId { get; set; }

        public string SecurityRoleCode { get; set; }
        public string SecurityRoleName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Generated Relationships
        [IgnoreDataMember]
        public virtual List<CompanySecurity> CompanySecurities { get; set; }
       
        public virtual List<SecurityRoleAccess> RoleAccess { get; set; }
       
        //public virtual List<SmartRole> RoleSmart { get; set; }
        #endregion

    }
}
