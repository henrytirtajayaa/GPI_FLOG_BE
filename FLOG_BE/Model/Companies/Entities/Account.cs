using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class Account
    {
        public Account()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public string AccountId { get; set; }
        public string Segment1 { get; set; }
        public string Segment2 { get; set; }
        public string Segment3 { get; set; }
        public string Segment4 { get; set; }
        public string Segment5 { get; set; }
        public string Segment6 { get; set; }
        public string Segment7 { get; set; }
        public string Segment8 { get; set; }
        public string Segment9 { get; set; }
        public string Segment10 { get; set; }
        public string Description { get; set; }
        public bool PostingType { get; set; }
        public bool NormalBalance { get; set; }
        public bool NoDirectEntry { get; set; }
        public bool Revaluation { get; set; }
        public bool Inactive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        #endregion


    }
}
