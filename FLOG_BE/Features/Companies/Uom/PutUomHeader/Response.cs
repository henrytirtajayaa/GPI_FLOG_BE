using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System.Collections.Generic;
using System;

namespace FLOG_BE.Features.Companies.Uom.PutUomHeader
{
    public class Response
    {
        public Guid UomHeaderId { get; set; }
        public string UomScheduleCode { get; set; }
        public string UomScheduleName { get; set; }

    }
}
