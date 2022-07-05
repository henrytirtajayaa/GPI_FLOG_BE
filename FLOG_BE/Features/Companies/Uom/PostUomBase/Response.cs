using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.Uom.PostUomBase
{
    public class Response
    {
        public Guid UomBaseId { get; set; }
        public string UomCode { get; set; }
        public string UomName { get; set; }
   
    }


}
