using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOG_BE.Model.Central.Entities
{
    public partial class SmartRole
    {
        public SmartRole()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid Id { get; set; }
        public Guid SmartviewId { get; set; }

        public Guid SecurityRoleId { get; set; }
        [NotMapped]
        public SmartView SmartView { get; set; }

        #endregion

    }
}
