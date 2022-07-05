using FLOG_BE.Helper.dto;
using FLOG_BE.Model.Central.Entities;
using System;
using System.Collections.Generic;

namespace FLOG_BE.Features.Authentication.DoLoginCompany
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string ErrorDesc { get; set; }
        public string Token { get; set; }
        public string CompanyId { get; set; }
        public string CompanySecurityId { get; set; }
        public List<MenuItem> Menus { get; set; }
        public Guid SessionId { get; set; }
    }
}
