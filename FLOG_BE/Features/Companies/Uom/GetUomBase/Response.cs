using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.Uom.GetUomBase
{
    public class Response
    {
        public List<ResponseItem> UomBases { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid UomBaseId { get; set; }
        public string UomCode { get; set; }
        public string UomName { get; set; }        
    }
}
