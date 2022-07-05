using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Companies.ChargeGroup.GetChargeGroup
{
    public class Response
    {
        public List<ResponseItem> ChargeGroup { get; set; }
        public ListInfo ListInfo { get; set; }
    }

    public class ResponseItem
    {
        public string ChargeGroupId { get; set; }
        public string ChargeGroupCode { get; set; }
        public string ChargeGroupName { get; set; }
        
    }
}
