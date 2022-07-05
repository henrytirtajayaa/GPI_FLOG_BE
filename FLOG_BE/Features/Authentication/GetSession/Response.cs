using FLOG_BE.Helper.dto;
using FLOG_BE.Model.Central.Entities;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Authentication.GetSession
{
    public class Response
    {
       // public List<ResponseItem> Sessions { get; set; }
        // public ListInfo ListInfo { get; set; }
        public string Id { get; set; }
        public string PersonId { get; set; }
        public string CompanySecurityId { get; set; }
        public string CompanyId { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }

    }
    public class ResponseItem
    {

        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public Guid CompanySecurityId { get; set; }
        public Guid CompanyId { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }


    }
  


}
