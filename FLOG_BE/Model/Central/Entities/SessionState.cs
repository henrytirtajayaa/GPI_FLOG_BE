using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FLOG_BE.Model.Central.Entities
{
    public partial class SessionState
    {
        public SessionState()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid Id { get; set; }
        public string PersonId { get; set; }
        public string CompanySecurityId { get; set; }
        public string CompanyId { get; set; }        
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }

        #endregion

        #region Generated Relationships


        #endregion

    }
}
