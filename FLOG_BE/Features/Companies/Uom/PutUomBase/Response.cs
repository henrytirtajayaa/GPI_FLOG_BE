using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;
using System;

namespace FLOG_BE.Features.Companies.Uom.PutUomBase
{
    public class Response
    {
        public Guid UomBaseId { get; set; }
        public string UomCode { get; set; }
        public string UomName { get; set; }

    }
}
