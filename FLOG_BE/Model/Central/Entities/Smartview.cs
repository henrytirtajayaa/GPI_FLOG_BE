using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FLOG_BE.Model.Central.Entities
{
    public partial class SmartView
    {
        public SmartView()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
     
        public Guid SmartviewId { get; set; }
        public string GroupName { get; set; }
        public string SmartTitle { get; set; }
        public string SqlViewName { get; set; }        
        public bool isFunction { get; set; }

        #endregion

        #region Generated Relationships


        #endregion

    }
}
