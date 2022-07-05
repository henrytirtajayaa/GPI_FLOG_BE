using FLOG_BE.Model.Central.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class ApprovalSetupDetail
    {
        public ApprovalSetupDetail()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid ApprovalSetupDetailId { get; set; }
        public Guid ApprovalSetupHeaderId { get; set; }
        public string Description { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? PersonCategoryId { get; set; }
        public int Level { get; set; }
        public bool HasLimit { get; set; }
        public decimal ApprovalLimit { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
       
        [NotMapped]
        public string PersonEmail { get; set; }
        [NotMapped]
        public string PersonFullName { get; set; }

        [NotMapped]
        public string UserGroupName { get; set; }
        [NotMapped]
        public string UserGroupCode { get; set; }
        #endregion


    }
}
