using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FLOG_BE.Model.Central.Entities
{
    public partial class SessionActivityLog
    {
        public SessionActivityLog()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid ActivityLogId { get; set; }
        public string PersonId { get; set; }
        public string CompanySecurityId { get; set; }
        public string CompanyId { get; set; }        
        public DateTime CreatedDate { get; set; }
        public string Comments { get; set; }
        public int Status { get; set; }

        #endregion
    }
}
