using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Model.Companies.Entities
{
    public class NumberFormatLastNo
    {
        public NumberFormatLastNo()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public Guid NumberFormatLastNoId { get; set; }
        public string DocumentId { get; set; }
        public int PeriodYear { get; set; }
        public int PeriodMonth { get; set; }
        public string LastNo { get; set; }
        public int LastIndex { get; set; }
        public DateTime? ModifiedDate { get; set; }

        #endregion
    }
}
