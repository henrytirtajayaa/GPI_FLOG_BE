using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.Uom.PostUomHeader
{
    public class Response
    {
        public Guid UomHeaderId { get; set; }
        public string UomScheduleCode { get; set; }
        public string UomScheduleName { get; set; }
    
    }


}
