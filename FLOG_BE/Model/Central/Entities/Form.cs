using System;
using System.Collections.Generic;

namespace FLOG_BE.Model.Central.Entities
{
    public partial class Form
    {
        public Form()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public string FormId { get; set; }

        public string FormName { get; set; }

        public string FormLink { get; set; }

        public string ParentId { get; set; }
        public string MenuIcon { get; set; }
        public int SortNo { get; set; }
        public string Module { get; set; }
        public bool IsVisible { get; set; }

        public bool InActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        #endregion

        #region Generated Relationships
        public virtual List<SecurityRoleAccess> RoleAccesses { get; set; }
        #endregion

    }
}
