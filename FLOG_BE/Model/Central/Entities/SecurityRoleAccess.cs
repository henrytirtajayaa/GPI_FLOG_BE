using System;
using System.Collections.Generic;

namespace FLOG_BE.Model.Central.Entities
{
    public partial class SecurityRoleAccess
    {
        public SecurityRoleAccess()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public string SecurityRoleAccessId { get; set; }
        public string SecurityRoleId { get; set; }

        public string FormId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public bool AllowNew { get; set; }
        public bool AllowOpen { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowPost { get; set; }
        public bool AllowPrint { get; set; }
        #endregion

        #region Generated Relationships
        public Form Form { get; set; }
         public SecurityRole SecurityRoles { get; set; }

        #endregion

    }
}
