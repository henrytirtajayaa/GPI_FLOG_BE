using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FLOG_BE.Model.Central.Entities
{
    public partial class Company
    {
        public Company()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public string CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string DatabaseId { get; set; }

        public string DatabaseAddress { get; set; }
        public string DatabasePassword { get; set; }

        public string CoaSymbol { get; set; }

        public int CoaTotalLength { get; set; }

        public bool InActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string SmtpServer { get; set; }
        
        public string SmtpPort { get; set; }
        
        public string SmtpUser { get; set; }
        
        public string SmtpPassword { get; set; }

        #endregion

        [NotMapped]
        public string CreatedByName { get; set; }
        [NotMapped]
        public string ModifiedByName { get; set; }

        #region Generated Relationships
        [IgnoreDataMember]
        public virtual List<CompanySecurity> CompanySecurities { get; set; }
        #endregion

    }
}
