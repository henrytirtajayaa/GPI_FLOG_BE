using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.Uom.GetUomHeader
{
    public class Response
    {
        public List<ResponseItem> UomHeaders { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public Guid UomHeaderId { get; set; }
        public string UomScheduleCode { get; set; }
        public string UomScheduleName { get; set; }
        public Guid UomBaseId { get; set; }
        public string UomBaseCode { get; set; }
        public string UomBaseName { get; set; }
        public List<ResponseDetail> UomDetails { get; set; }
    }

    public class ResponseDetail
    {
        public Guid UomDetailId { get; set; }
        public string UomCode { get; set; }
        public string UomName { get; set; }
        public decimal EquivalentQty { get; set; }
    }
}
